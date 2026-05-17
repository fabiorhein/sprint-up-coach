# Spec 001: Importação de Atividade do Strava

**Status**: MVP V1 - Sprint 1  
**Prioridade**: CRÍTICA  
**Complexidade**: Média  
**Tokens Estimados**: ~2000 (Gemini Flash)

---

## 📌 Objetivo

Criar um endpoint HTTP que receba um payload de atividade de corrida vindo da API do Strava e o processe seguindo regras rígidas de validação de negócio.

Este endpoint será usado pelo Strava Webhook para sincronizar corridas do usuário em tempo real.

---

## 🔌 Entrada (Input)

### Payload JSON esperado do Strava

```json
{
  "id": 123456789,
  "type": "Run",
  "distance": 10000.0,
  "moving_time": 3600,
  "elapsed_time": 3720,
  "start_date_local": "2026-05-15T08:30:00Z",
  "athlete_id": 987654
}
```

### Descrição dos campos:

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `id` | Long | ✅ | ID único da atividade no Strava |
| `type` | String | ✅ | Tipo de atividade (deve ser exatamente "Run") |
| `distance` | Double | ✅ | Distância em metros |
| `moving_time` | Int | ✅ | Tempo em movimento (segundos) |
| `elapsed_time` | Int | ❌ | Tempo total decorrido (ignorar) |
| `start_date_local` | DateTime | ✅ | Data/hora local de início da atividade |
| `athlete_id` | Long | ✅ | ID do atleta (usuário) |

---

## 🎯 Regras de Negócio

### 1. Validação de Tipo
- **Apenas "Run"** deve ser processado
- Se `type != "Run"` (ex: Bike, Walk, Swim), **retornar HTTP 200 OK sem salvar**
  - Motivo: O webhook do Strava tenta reenviar se receber erro. Ignorar silenciosamente é mais seguro.

### 2. Validação de Dados
- `distance` deve ser **> 0** metros
- `moving_time` deve ser **> 0** segundos
- Se qualquer validação falhar, **retornar HTTP 400 Bad Request**

### 3. Cálculo de Pace (Ritmo Médio)

**Definição**: Ritmo é o tempo gasto para correr 1 quilômetro.

**Fórmula**:
```
Pace (min/km) = (moving_time / 60) / (distance / 1000)
```

**Exemplo**:
- Distância: 10.000 metros (10 km)
- Tempo em movimento: 3.600 segundos (60 minutos)
- Pace = (3600 / 60) / (10000 / 1000) = 60 / 10 = **6.0 minutos por km**

### 4. Velocidade Média (Derivado)

**Definição**: Distância percorrida por hora.

**Fórmula**:
```
Speed (km/h) = (distance / 1000) / (moving_time / 3600)
```

**Exemplo**:
- Speed = (10000 / 1000) / (3600 / 3600) = 10 / 1 = **10 km/h**

### 5. Prevenção de Duplicatas
- Se uma atividade com o mesmo `id` do Strava já existe no BD, **atualizar** em vez de criar nova
- Usar campo `StravaActivityId` (String) como chave de deduplicação

### 6. Persistência no Banco
- Salvar a atividade no PostgreSQL com:
  - `Id`: UUID gerado automaticamente (chave primária)
  - `StravaActivityId`: ID do Strava (string, índice para busca rápida)
  - `Distance`: double (metros)
  - `MovingTimeSeconds`: int
  - `PaceMinPerKm`: double (calculado)
  - `SpeedKmPerHour`: double (calculado)
  - `StartDateLocal`: DateTime
  - `CreatedAt`: DateTime (agora)
  - `UpdatedAt`: DateTime (agora)

---

## 📤 Respostas (HTTP)

### ✅ HTTP 200 OK - Sucesso (Salvo)
```json
{
  "success": true,
  "message": "Atividade importada com sucesso",
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "stravaActivityId": "123456789",
    "distance": 10000.0,
    "pace": 6.0,
    "speed": 10.0,
    "startDate": "2026-05-15T08:30:00Z"
  }
}
```

### ✅ HTTP 200 OK - Ignorado (Tipo não é Run)
```json
{
  "success": true,
  "message": "Atividade ignorada (tipo não é Run)",
  "data": null
}
```

### ❌ HTTP 400 Bad Request - Validação falhou
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "detail": "Distância deve ser maior que 0 metros"
}
```

### ❌ HTTP 409 Conflict - Atividade duplicada (Futuro)
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.8",
  "title": "Duplicate Activity",
  "status": 409,
  "detail": "Esta atividade já foi importada em 2026-05-15 às 08:30"
}
```

---

## 🔒 Endpoint HTTP

### Rota
```
POST /api/v1/activities/import
```

### Headers
```
Content-Type: application/json
```

### Autenticação
- ❌ **Sem autenticação por enquanto** (será adicionado na Spec 002)
- O webhook do Strava virá com um header secreto (será validado depois)

---

## 🧪 Critérios de Aceitação (Testes Obrigatórios)

Todo teste deve passar para a Spec ser considerada **COMPLETA**.

### Teste 1: Deve calcular Pace corretamente
```
Dado: Distância 10.000 metros, Tempo 3.600 segundos (1 hora)
Quando: CreateFromStrava() é chamado
Então: PaceMinPerKm deve ser exatamente 6.0 min/km
```

### Teste 2: Deve calcular Speed corretamente
```
Dado: Distância 10.000 metros, Tempo 3.600 segundos
Quando: CreateFromStrava() é chamado
Então: SpeedKmPerHour deve ser exatamente 10.0 km/h
```

### Teste 3: Deve rejeitar não-corridas
```
Dado: type = "Bike"
Quando: CreateFromStrava() é chamado
Então: Retornar Failure (ou ignorar com sucesso silencioso no endpoint)
```

### Teste 4: Deve validar distância > 0
```
Dado: distance = 0 ou negativo
Quando: CreateFromStrava() é chamado
Então: Retornar erro "Distância deve ser maior que 0"
```

### Teste 5: Deve validar tempo > 0
```
Dado: moving_time = 0 ou negativo
Quando: CreateFromStrava() é chamado
Então: Retornar erro "Tempo deve ser maior que 0"
```

### Teste 6: Deve salvar corretamente no banco
```
Dado: DTO válido com id 123456789
Quando: ImportActivityHandler.Handle() é executado
Então: 
  - StravaActivity deve estar salvo no DB
  - Ao buscar por StravaActivityId "123456789", deve encontrar
  - CreatedAt deve ser agora
```

### Teste 7: Deve idempotência (mesma atividade 2x = 1 registro)
```
Dado: Mesma atividade importada 2 vezes
Quando: Segunda importação ocorre
Então: 
  - Deve atualizar o registro existente
  - Não deve criar duplicata
  - UpdatedAt deve ser a data da segunda importação
```

### Teste 8: Deve retornar DTO formatado
```
Dado: atividade salva
Quando: Endpoint retorna 200
Então: JSON contém id, stravaActivityId, distance, pace, speed, startDate
```

---

## 🏗️ Arquitetura Esperada

Após implementação, a estrutura será:

```
src/Modules/Strava/
├── Domain/
│   ├── StravaActivity.cs              (Entidade com Factory)
│   └── IStravaRepository.cs           (Interface)
│
├── Features/
│   └── ImportActivity/
│       ├── ImportActivityEndpoint.cs   (Minimal API - HTTP)
│       ├── ImportActivityHandler.cs    (Lógica - Caso de Uso)
│       ├── ImportActivityRequest.cs    (DTO de Entrada)
│       └── ImportActivityDto.cs        (DTO de Saída)
│
└── Infrastructure/
    └── StravaRepository.cs            (Implementação)

tests/Modules/Strava/
└── Features/
    └── ImportActivityTests.cs         (Testes)
```

---

## 📝 Exemplo de Como o Aider Deve Executar

### Passo 1: Ler a Spec
```bash
/add specs/001-strava-import.md
```

### Passo 2: Pedir Implementação
```bash
Implemente a Spec 001 seguindo CONVENTIONS.md e AGENTS.md. 
Crie a entidade StravaActivity com Factory Method, o Handler, o Endpoint, 
o Repositório e os testes. 
Rode 'make harness' e corrija até passar.
```

### Passo 3: Aider executa internamente
- Cria a entidade com validações
- Implementa o cálculo de Pace e Speed
- Cria o endpoint Minimal API
- Cria os testes xUnit
- Executa `make harness`
- Se passar, faz commit automático

---

## 🚀 Dependências Necessárias

Já devem estar no `.csproj`:
- ✅ `Microsoft.EntityFrameworkCore`
- ✅ `Microsoft.EntityFrameworkCore.PostgreSQL`
- ✅ `xunit`
- ✅ `FluentAssertions`
- ✅ `Moq`

---

## 📚 Referências Externas

- [Strava API Docs](https://developers.strava.com/docs/reference/) - Documentação oficial
- [RFC 7231 - HTTP Semantics](https://tools.ietf.org/html/rfc7231#section-6.5) - Códigos HTTP
- [Problem Details RFC 7807](https://datatracker.ietf.org/doc/html/rfc7807) - Erro Responses

---

## ✅ Critério de Conclusão

A Spec 001 é **COMPLETA** quando:

1. ✅ Todos os 8 testes passam (`make harness`)
2. ✅ O código segue CONVENTIONS.md
3. ✅ Cálculos de Pace e Speed estão corretos (validar com exemplos)
4. ✅ Banco de dados salva corretamente (sem duplicatas)
5. ✅ Endpoint retorna JSON formatado
6. ✅ Git tem commit automático
7. ✅ Nenhum warning no build

**Se tudo passar acima: A feature está pronta para produção.**

---

**Próxima Spec**: 002-google-oauth-login.md (Autenticação de usuários)