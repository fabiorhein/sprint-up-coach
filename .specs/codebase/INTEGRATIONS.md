# INTEGRATIONS.md - External Providers

## 🏃 Agnostic Ingestion Architecture
To prevent tight coupling to third-party endpoints, all ingestion features must consume an interface declared in the Domain called `IActivityProvider`.

## 1. Strava Provider Implementation
- **Type**: REST / OAuth 2.0 Client.
- **Base Endpoint**: `https://www.strava.com/api/v3`.
- **Scope**: Read-only operations for user activities.
- **Resilience**: Simple retry loop on HTTP status `429` (Rate Limited).

## ✉️ Transactional Email Provider
- **Abstraction**: `IEmailService` defined in User Management Domain.
- **MVP Implementation**: Lightweight wrapper targeting free-tier Resend/SendGrid SMTP or HTTP client API to issue verification links.