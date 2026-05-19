# CONVENTIONS.md - Code Patterns

## 🎯 1. Feature Structure (Vertical Slice)
Every use case is self-contained. For example: `src/Modules/UserManagement/Features/RegisterUser/`
- `RegisterUserEndpoint.cs` -> Minimal API mapping (`MapPost`), inputs validation, and returns HTTP.
- `RegisterUserHandler.cs` -> Contains 100% of the single-use business logic.
- `RegisterUserDto.cs` -> In/Out request and response contracts.

## 🛑 2. Strict Bans (Anti-Patterns for this MVP)
- ❌ **No AutoMapper**: Write explicit extension methods (e.g., `.ToDto()`) for conversions. It saves tokens and prevents model mapping hallucination.
- ❌ **No Generic Repository Pattern**: Inject the `SprintUpDbContext` directly into Handlers or specific Infrastructure implementations.
- ❌ **No Controllers**: Always use ASP.NET Core Minimal API endpoint extensions.

## 📡 3. API & Response Conventions
- **Routing**: Follow `/api/v1/{resource}` syntax.
- **Errors**: Always implement RFC 7231 `ProblemDetails` via `Results.Problem()`.
- **i18n**: Endpoints must read the `Accept-Language` header to process outputs (PT-BR, EN, ES).