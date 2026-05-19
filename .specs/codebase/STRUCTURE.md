# STRUCTURE.md - Project Organization

**Purpose**: Understand where everything is and how to navigate  
**Last Updated**: May 15, 2026

---

## 📂 Full Directory Tree

```
sprintup/
├── Makefile                           # Automation gate (make harness)
├── docker-compose.yml                 # Local Infra (PostgreSQL 16)
├── src/                               # Application Core (Full-Stack Web App)
│   ├── Modules/                       # Modular Monolith Root
│   │   ├── UserManagement/            # Auth, Profile, Blazor Auth Pages
│   │   └── Integration/               # Ingestion Core, Providers & Sync UI
│   ├── Shared/                        # Shared context (DbContext, MainLayout.razor, NavMenu.razor)
│   └── wwwroot/                       # Static Web Assets (Global CSS, Images, Favicon)
├── tests/                             # Mirrors src/ directory tree (Includes UI Tests)
└── .specs/                            # SDD Source of Truth
├── project/                       # Core project state (PROJECT, ROADMAP, STATE)
├── codebase/                      # This technical documentation
└── features/                      # Markdown specifications for the AI
```

---

## 📋 File Placement Checklist
- **UI Razor Pages & Layouts** -> `src/Modules/[Module]/Features/[Feature]/[Feature]Page.razor`
- **Component Specific Style** -> `src/Modules/[Module]/Features/[Feature]/[Feature]Page.razor.css`
- **Domain Logic** -> `src/Modules/[Module]/Features/[Feature]/Handler.cs`
- **UI & Component Tests** -> `tests/Modules/[Module]/Features/[Feature]/[Feature]PageTests.cs`