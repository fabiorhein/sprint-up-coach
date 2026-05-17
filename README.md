# 🏃 SprintUp - Seu Laboratório de SDD com Aider

**SprintUp** é um aplicativo de treino de corridas de rua que integra dados do Strava com análise de desempenho.

Mais importante: **é seu laboratório prático de Spec-Driven Development (SDD) com Aider**.

---

## 🎯 Objetivo Deste Projeto

Demonstrar como usar **Aider** + **Specs** + **Harness** para desenvolver um SaaS backend em **.NET 8** com:

✅ Consumo mínimo de tokens (Gemini grátis)  
✅ Código limpo (DDD, SOLID, Clean Code)  
✅ Teste automático (xUnit + FluentAssertions)  
✅ Git automático (commits pelo Aider)  
✅ Hardware levíssimo (8GB RAM, monolito modular)

---

## 📚 Documentação

| Arquivo | O Quê | Leia Se... |
|---------|-------|-----------|
| **QUICKSTART.md** | Guia passo a passo (15 min) | Você quer começar AGORA |
| **AGENTS.md** | Contexto para IA | Você quer entender como Aider funciona |
| **CONVENTIONS.md** | Regras de código C# | Você quer saber o padrão esperado |
| **specs/001-strava-import.md** | Primeira feature | Você quer ver um exemplo de Spec |
| **Makefile** | Automação | Você quer rodar comandos rápidos |

---

## 🚀 Start Rápido (3 passos)

### 1️⃣ Clone os arquivos deste projeto

```bash
mkdir sprintup && cd sprintup
# Copie CONVENTIONS.md, AGENTS.md, Makefile, docker-compose.yml, specs/
git init
```

### 2️⃣ Levante o PostgreSQL

```bash
docker-compose up -d
```

### 3️⃣ Abra o Aider e peça a implementação

```bash
export GEMINI_API_KEY="sua_chave"
aider --model gemini/gemini-1.5-flash
```

Dentro do Aider:
```
/add CONVENTIONS.md
/add specs/001-strava-import.md
Implemente a Spec 001 seguindo CONVENTIONS.md. Rode make harness e corrija até passar.
```

**Pronto!** ✅ Em 5-10 minutos seu backend estará pronto com testes passando.

👉 **[Ver guia detalhado → QUICKSTART.md](./QUICKSTART.md)**

---

## 🏗️ Arquitetura

```
sprintup/
├── src/SprintUp.csproj              (Seu código)
│   └── Modules/
│       ├── Strava/                  (Módulo: Integração Strava)
│       └── Users/                   (Futuro: Autenticação)
│
├── tests/SprintUp.Tests.csproj      (Seus testes)
│
├── specs/                           (Suas especificações)
│   └── 001-strava-import.md
│
├── AGENTS.md                        (Instruções para IA)
├── CONVENTIONS.md                   (Padrões de código)
├── Makefile                         (Automação)
└── docker-compose.yml               (PostgreSQL)
```

**Filosofia**: Um projeto único, organizado por Features. Sem Clean Architecture pesada.

---

## 📋 Roadmap (Specs Planejadas)

- ✅ **Spec 001**: Importação de Atividade Strava
- 🔄 **Spec 002**: Autenticação Google OAuth
- 🔄 **Spec 003**: Dashboard com Métricas
- 🔄 **Spec 004**: Alertas de Sobrecarga
- 🔄 **Spec 005**: IA para Recomendações

---

## 🛠️ Tech Stack

| Camada | Tecnologia |
|--------|-----------|
| **Backend** | ASP.NET Core 8 |
| **BD** | PostgreSQL 16 |
| **Testes** | xUnit + FluentAssertions |
| **IA** | Aider + Gemini 1.5 Flash |
| **Infra** | Docker Compose |
| **Versionamento** | Git |

---

## 📊 Resultados Esperados

Após completar as 5 specs, você terá:

**Backend Funcional**:
- ✅ Autenticação com Google
- ✅ Sincronização Strava em tempo real
- ✅ Dashboard com 5+ métricas
- ✅ Alertas automáticos
- ✅ Integração com IA

**Conhecimento Adquirido**:
- ✅ SDD (Spec-Driven Development)
- ✅ Harness (Testes Automáticos)
- ✅ DDD Tático em C#
- ✅ Como usar Aider
- ✅ Monolito Modular

**Zero Custo**:
- ✅ Gemini Flash (grátis)
- ✅ PostgreSQL (grátis)
- ✅ .NET (grátis)
- ✅ Docker (grátis)

---

## 🎓 O Que é SDD?

### Desenvolvimento Tradicional ❌
```
"Crie um sistema de autenticação JWT"
  ↓
AI gera código aleatório
  ↓
Você gasta 10 tokens reescrevendo
  ↓
Resultado: Lixo
```

### SDD (Spec-Driven) ✅
```
Spec: "Login com email, hash bcrypt, tokens 15min"
  ↓
AI lê a Spec exatamente
  ↓
AI escreve código limpo, testado
  ↓
Resultado: Ouro
```

---

## 💡 Por Que Isso Importa Para Você

1. **Você não tem GPU caros** → Aider + Gemini grátis rodam tudo
2. **Seu Mac Mini tem 8GB** → Monolito modular usa pouca RAM
3. **Você está aprendendo** → SDD ensina arquitetura real
4. **Quer ser ágil** → Specs + Aider = velocidade máxima

---

## 🚦 Como Começar Agora

### Opção A: Eu Quero Aprender (Recomendado)
👉 [Leia QUICKSTART.md](./QUICKSTART.md) (15 minutos)

### Opção B: Eu Quero Entender Tudo
👉 [Leia AGENTS.md](./AGENTS.md) (30 minutos)

### Opção C: Eu Quero Ver o Código
👉 [Leia specs/001-strava-import.md](./specs/001-strava-import.md)

---

## 🆘 Preciso de Ajuda?

| Problema | Solução |
|----------|---------|
| "Aider não funciona" | [Ver QUICKSTART.md - Troubleshooting](#) |
| "Não entendo a Spec" | [Leia AGENTS.md - Como Specs Funcionam](#) |
| "Testes falharam" | `make test` e leia o erro |
| "Quero otimizar tokens" | [AGENTS.md - Economizar Tokens](#) |

---

## 📞 Contribuindo

Quer melhorar as Specs ou Conventions?

1. Fork este projeto
2. Crie uma branch: `git checkout -b melhoria/sua-ideia`
3. Commita: `git commit -m "melhoria: descricao"`
4. Push: `git push origin melhoria/sua-ideia`
5. Abra um Pull Request

---

## 📝 Licença

MIT - Use livremente para aprender e criar.

---

## 🎉 Próximas Palavras

Você está entrando em um laboratório onde:

✅ IA não é "magia" → É **arquitetura** clara  
✅ Desenvolvimento não é lento → É **automático**  
✅ Testes não são opcionais → São **obrigatórios**  
✅ Código não é caótico → É **modular e limpo**

**Bem-vindo ao futuro do desenvolvimento com IA.**

👉 **[Comece agora → QUICKSTART.md](./QUICKSTART.md)**

---

**Versão**: 1.0  
**Última atualização**: Maio 2026  
**Criado para**: Desenvolvedores aprendendo SDD + Aider

Good luck! 🚀