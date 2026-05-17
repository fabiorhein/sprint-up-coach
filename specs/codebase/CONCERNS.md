# Preocupações Técnicas e Riscos (CONCERNS)

## 1. Gestão de Memória (Hardware Limitado)
- **Risco:** O ambiente de laboratório roda localmente em hardware de 8GB de RAM.
- **Mitigação:** Evitar o uso de ferramentas pesadas de profile ativas em background. Queries do EF Core que sejam apenas para leitura devem usar obrigatoriamente `.AsNoTracking()`.

## 2. Concorrência no PostgreSQL
- **Risco:** Webhooks do Strava podem disparar múltiplos eventos simultâneos para a mesma atividade se o usuário editar o treino repetidamente.
- **Mitigação:** Garantir que o ID da atividade do Strava possua um índice `UNIQUE` no banco de dados para evitar inserções duplicadas via concorrência.

## 3. Segurança de Tokens
- **Risco:** Vazamento de `access_token` de usuários do Strava.
- **Mitigação:** Nunca salvar os tokens em texto limpo para logs de debug. Usar variáveis de ambiente (`.env`) para chaves criptográficas do sistema.