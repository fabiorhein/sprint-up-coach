# STACK.md - Technical Stack Details

**Status**: Fixed for MVP V1  
**Philosophy**: Native Full-Stack .NET features over third-party packages to minimize code footprint and token overhead.

---

## 🏗️ Core Backend
- **Runtime**: .NET 8.0 (LTS)
- **Language**: C# 12
- **Framework**: ASP.NET Core Minimal APIs (Strictly no Controllers)
- **Dependency Injection**: Native `IServiceCollection` (Centralized registration)

## 🎨 Core Frontend (UI)
- **Framework**: Blazor Web App (.NET 8)
- **Render Mode**: Static Server-Side Rendering (SSR) by default for fast initial loads, combined with `InteractiveServer` component-level rendering for real-time dashboards and dynamic forms.
- **Styling & Responsiveness**: Vanilla Bootstrap 5 (Mobile-First grid system embedded in the native .NET template) + Blazor CSS Isolation (`.razor.css`).
- **State Management**: Native Cascading Parameters and scoped UI services. No heavy third-party state libraries (e.g., Fluxor).

## 🗄️ Database & Persistence
- **Engine**: PostgreSQL 16 (Alpine via Docker Compose)
- **ORM**: Entity Framework Core 8.0 (Code-First)
- **Primary Keys**: UUID (Guid) generated via database `gen_random_uuid()`

## 🛡️ Security & Auth
- **Identity**: ASP.NET Core Identity + Cookie Authentication for Blazor SSR/Interactive flows.

## 🧪 Testing & Harness
- **Framework**: xUnit
- **Assertions**: FluentAssertions
- **Mocks**: Moq (Only for external I/O and interfaces)
- **UI Testing**: bUnit (For Blazor component lifecycle and markup validation)

## ⚙️ Automation Scripts
- **Build & Test Enforcer**: `make harness` (Triggers format check, build, backend tests, and UI component tests)
- **Database Migrations**: `make migrate`