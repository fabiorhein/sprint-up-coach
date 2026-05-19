# ARCHITECTURE.md - System Design

## Architectural Style: Full-Stack Modular Monolith (Single Project)
The system is built as a single `.NET` web project (`src/`) containing both the backend Minimal APIs and the Blazor UI. It is divided into completely isolated modules. No microservices, no separate frontend repository, and no Clean Architecture layers.

---

## 📁 Module Component Layout (Full-Stack Vertical Slice)
Every module inside `src/Modules/[ModuleName]/` must strictly contain only three folders. UI elements are co-located within the features:

1. **Domain/**: Rich entities, Value Objects, custom domain exceptions, and core interface contracts. **Zero dependencies on UI or Infrastructure.**
2. **Features/**: Vertical slice use cases organized by folder (`Features/[FeatureName]/`). It encapsulates the entire execution flow:
   - `[Feature]Endpoint.cs` -> API route mapping (if needed for integration/mobile).
   - `[Feature]Handler.cs` -> Business logic execution.
   - `[Feature]Page.razor` -> The Blazor UI view component (Responsive for mobile/desktop).
   - `[Feature]Page.razor.css` -> Local component styles (CSS Isolation).
   - `[Feature]Dto.cs` -> Data contracts between UI, Handler, and API.
3. **Infrastructure/**: EF Core entity configurations, DbContext extension configurations, and implementations of domain interfaces.

---

## 🛑 Isolation & Communication Rules (Crucial for AI)
1. **Co-location Principle**: Keep the UI page, the validation logic, and the handler in the same Feature folder. This enables the AI agent to edit the full stack in a single prompt context.
2. **Pure C# Interface Communication**: Inter-module communication happens strictly via domain interfaces registered in the Dependency Injection container.