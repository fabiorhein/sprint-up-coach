# Integrações Externas - SprintUp

## 1. Strava API
- **Protocolo:** REST / OAuth 2.0
- **Base URL:** `https://www.strava.com/api/v3`
- **Autenticação:** Bearer Token via Header `Authorization: Bearer [access_token]`.

### Fluxo de Webhooks (Webhook Events)
- O Strava envia atualizações via `POST` para o nosso endpoint de Webhook.
- Formato esperado do Handshake de validação do Webhook (`GET`):
  Deve responder com JSON `{ "hub.challenge": "valor_recebido" }` quando o Strava validar a URL.

### Limites de Rate Limit (Atenção IA)
- 100 requisições a cada 15 minutos.
- 1000 requisições por dia.
- **Regra:** Implementar resiliência simples (Retry lógico) se receber HTTP 429.