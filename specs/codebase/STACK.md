# STACK.md - Technical Stack Details

**Last Updated**: May 15, 2026  
**Stack Type**: Full-Stack (.NET + PostgreSQL)  
**Scale**: MVP (< 1k users)

---

## 🏗️ Backend

### Runtime & Framework
- **Runtime**: .NET 8.0 (LTS)
- **Language**: C# 12
- **Framework**: ASP.NET Core
- **API Style**: Minimal APIs (no Controllers)
- **Pattern**: Monolito Modular + Feature Folders

### Web Server
- **Host**: Kestrel (built-in)
- **Port**: 5000 (local), configurable via env
- **HTTPS**: Self-signed cert (local), real cert (prod)

### Dependency Injection
- **Container**: Built-in IServiceCollection
- **Lifetime**: Scoped (default per request)
- **Configuration**: Program.cs

---

## 🗄️ Database

### RDBMS
- **Engine**: PostgreSQL 16 (Alpine)
- **Connection**: EF Core DbContext
- **Port**: 5432 (local via Docker)
- **Host**: `postgres` (docker-compose network)

### ORM
- **Framework**: Entity Framework Core 8.0
- **Migrations**: Code-first via `dotnet ef migrations`
- **Queries**: LINQ + Raw SQL (when needed)
- **Transactions**: Implicit per SaveChanges()

### Database Design
- **Primary Keys**: UUID (Guid) - generated via `gen_random_uuid()`
- **Foreign Keys**: Direct references
- **Indexes**: TBD (added when query slow)
- **Constraints**: Check constraints for business rules
- **Soft Deletes**: No (hard deletes only for now)

---

## 🧪 Testing

### Framework
- **Unit Tests**: xUnit
- **Assertions**: FluentAssertions
- **Mocking**: Moq
- **Test Organization**: By feature

### Test Types
| Type | Tool | When | Example |
|------|------|------|---------|
| **Unit** | xUnit + FluentAssertions | Test domain logic | Pace calculation |
| **Integration** | xUnit + InMemory DB | Test DB persistence | Save activity |
| **API** | curl / Postman | Test endpoints | POST /api/v1/activities/import |

### Test Patterns
- **Naming**: `Should{action}When{condition}`
- **Structure**: Arrange-Act-Assert (AAA)
- **Coverage Target**: > 80% (focus on domain + critical paths)
- **Database**: Real PostgreSQL (not mocked)

---

## 📦 NuGet Dependencies

### Core
```
Microsoft.AspNetCore.App (bundled)
System.Reflection.Metadata
System.Text.Json (built-in)
```

### Database
```
Microsoft.EntityFrameworkCore.PostgreSQL 8.0
Microsoft.EntityFrameworkCore.Tools 8.0 (for migrations)
```

### Testing
```
xunit 2.6.x
FluentAssertions 6.x
Moq 4.x
Microsoft.EntityFrameworkCore.InMemory (for test DB)
```

### Validation (Optional)
```
FluentValidation 11.x (if we add complex validation)
```

### NOT Using
- ❌ AutoMapper (use manual mapping)
- ❌ MediatR (no need for CQRS yet)
- ❌ Serilog (ILogger built-in)
- ❌ Swagger/OpenAPI (Minimal APIs have built-in support)

---

## 🐳 Infrastructure

### Containerization
- **Container Runtime**: Docker
- **Compose**: docker-compose.yml
- **Services**:
  - `postgres` - PostgreSQL 16 Alpine
  - `pgadmin` - Web UI for database management

### Local Development
```bash
# Start infrastructure
docker-compose up -d

# Stop infrastructure
docker-compose down

# View logs
docker-compose logs -f postgres

# Access PgAdmin
http://localhost:5050
```

### Production (Future)
- **Database**: AWS RDS PostgreSQL
- **API**: AWS ECS / App Service
- **Container Registry**: AWS ECR / Docker Hub

---

## 🔐 Security (MVP Level)

### Authentication
- **Spec 002**: Google OAuth 2.0 integration
- **Tokens**: JWT (15 min access, 7 day refresh)
- **Storage**: Cookies (secure, httpOnly)

### Encryption
- **HTTPS**: TLS 1.3+
- **Passwords**: N/A (OAuth only)
- **Secrets**: Environment variables (GitHub Actions)

### Rate Limiting
- **Status**: Not implemented (low user count)
- **Plan**: Add Redis + AspNetCoreRateLimit when needed

---

## 📊 Performance Targets

### API Latency
- **Endpoint**: < 100ms (p95)
- **Database Query**: < 50ms (p95)
- **Goal**: Responsive user experience

### Database Performance
- **Indexes**: Add when query slow
- **Caching**: None yet (PostgreSQL caches, Redis later)

### Build Time
- **Compile**: < 10s
- **Test**: < 20s (all tests)
- **Target**: `make harness` in < 30s

---

## 🚀 Deployment Pipeline (Future)

### Local Development
```
Code → Git → Aider → make harness ✅ → Commit → Push
```

### CI/CD (Post-MVP)
```
Push → GitHub Actions → Build → Test → Deploy → ECS
```

### Monitoring (Post-MVP)
- **Logs**: CloudWatch / Datadog
- **Metrics**: Prometheus / New Relic
- **Tracing**: Application Insights / Jaeger

---

## 📝 Configuration Management

### Environment Variables
```bash
# Local (.env file - NOT in git)
ASPNETCORE_ENVIRONMENT=Development
DATABASE_CONNECTION=postgresql://user:pass@localhost:5432/sprintup_dev
GOOGLE_OAUTH_CLIENT_ID=xxx
GOOGLE_OAUTH_CLIENT_SECRET=xxx
JWT_SECRET_KEY=xxx

# Production (GitHub Secrets)
Same as above, with prod values
```

### Secrets Management (Future)
- **Local**: .env file (git-ignored)
- **Production**: AWS Secrets Manager / Azure Key Vault

---

## 🔧 Developer Tools

### Local Tooling
- **IDE**: VS Code / Visual Studio / Rider
- **Git**: Git CLI
- **Terminal**: Bash / PowerShell / Zsh
- **HTTP Client**: curl / Postman / Thunder Client
- **Database GUI**: pgAdmin (included in compose)

### Automation
- **Build**: `make build`
- **Test**: `make test`
- **Harness**: `make harness`
- **Migrate**: `make migrate`
- **Run**: `make run`

### Formatting & Linting
- **Formatter**: `dotnet format` (built-in)
- **Linter**: `dotnet analyzers` (built-in)
- **Pre-commit**: None yet (low priority)

---

## 📚 Documentation

### Developer Docs
- `README.md` - Quick start
- `QUICKSTART.md` - First feature walkthrough
- `.specs/codebase/CONVENTIONS.md` - Code patterns
- `.specs/codebase/ARCHITECTURE.md` - System design
- `.specs/codebase/TESTING.md` - Test patterns

### API Docs
- **OpenAPI**: Auto-generated by Minimal APIs
- **Postman**: Collection (TBD - to be created)
- **Examples**: Curl commands in each spec

---

## 🎯 Stack Philosophy

### Principles
1. **Minimal Dependencies**: Only what's needed (no bloat)
2. **Built-in Over Packages**: Use .NET built-in when possible
3. **Leverage Minimal APIs**: Less code = fewer places for bugs
4. **Fast Iteration**: Quick builds, easy to understand
5. **Test Everything**: No feature without tests

### Trade-offs
- **Repository Pattern**: No (use DbContext directly)
- **CQRS**: No (overkill for MVP)
- **Event Sourcing**: No (not needed yet)
- **Microservices**: No (monolito modular sufficient)

---

**Version**: 1.0  
**Last Updated**: May 15, 2026  
**Maintained By**: Fabio