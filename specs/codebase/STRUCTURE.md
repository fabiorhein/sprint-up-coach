# STRUCTURE.md - Project Organization

**Purpose**: Understand where everything is and how to navigate  
**Last Updated**: May 15, 2026

---

## 📂 Full Directory Tree

```
sprintup/
│
├── 📄 README.md                          # Entry point for new devs
├── 📄 Makefile                           # Automation commands
├── 📄 docker-compose.yml                 # Local infrastructure
├── 📄 .gitignore                         # Git excludes
│
├── 📁 .specs/                            # 🚀 ALL SPECS & DOCS HERE
│   ├── 📁 project/                       # Project-level
│   │   ├── PROJECT.md                    # Vision, goals, roadmap
│   │   ├── ROADMAP.md                    # Feature planning
│   │   └── STATE.md                      # Persistent memory
│   │
│   ├── 📁 codebase/                      # Code organization docs
│   │   ├── STACK.md                      # Tech stack details
│   │   ├── ARCHITECTURE.md               # System design [TODO]
│   │   ├── CONVENTIONS.md                # Code patterns
│   │   ├── STRUCTURE.md                  # This file
│   │   ├── TESTING.md                    # Test patterns [TODO]
│   │   ├── INTEGRATIONS.md               # External APIs [TODO]
│   │   └── CONCERNS.md                   # Tech debt, risks [TODO]
│   │
│   ├── 📁 features/                      # Feature specs
│   │   ├── 📁 strava-import/             # Feature: Import Strava activities
│   │   │   ├── spec.md                   # Requirements [READY]
│   │   │   ├── design.md                 # [Skipped - straightforward]
│   │   │   ├── tasks.md                  # [Skipped - implicit]
│   │   │   └── context.md                # [Skipped - no ambiguity]
│   │   │
│   │   ├── 📁 google-oauth/              # Feature: Google authentication
│   │   │   ├── spec.md                   # [TODO - to be written]
│   │   │   ├── design.md                 # [TODO if complex]
│   │   │   └── context.md                # [TODO if needed]
│   │   │
│   │   ├── 📁 dashboard/                 # Feature: User dashboard
│   │   │   ├── spec.md                   # [TODO - depends on 001 & 002]
│   │   │   └── ...
│   │   │
│   │   └── 📁 [other-features]/          # Future specs...
│   │
│   └── 📁 quick/                         # Quick-mode tasks (ad-hoc)
│       └── NNN-slug/
│           ├── TASK.md
│           └── SUMMARY.md
│
├── 📁 src/                               # 💾 YOUR CODE (.NET)
│   ├── SprintUp.csproj                   # Project file
│   ├── Program.cs                        # App entry point
│   ├── SprintUpDbContext.cs              # Database context
│   │
│   └── 📁 Modules/                       # Organized by feature
│       ├── 📁 Strava/                    # Strava module
│       │   ├── 📁 Domain/
│       │   │   ├── StravaActivity.cs     # Entity
│       │   │   ├── IStravaRepository.cs  # Interface
│       │   │   └── [other domain models]
│       │   │
│       │   ├── 📁 Features/
│       │   │   └── 📁 ImportActivity/
│       │   │       ├── ImportActivityEndpoint.cs
│       │   │       ├── ImportActivityHandler.cs
│       │   │       ├── ImportActivityRequest.cs
│       │   │       └── ImportActivityDto.cs
│       │   │
│       │   └── 📁 Infrastructure/
│       │       └── StravaRepository.cs   # Data access
│       │
│       ├── 📁 Users/                     # Users module (future)
│       │   ├── 📁 Domain/
│       │   ├── 📁 Features/
│       │   └── 📁 Infrastructure/
│       │
│       └── 📁 [other-modules]/
│
├── 📁 tests/                             # 🧪 YOUR TESTS
│   ├── SprintUp.Tests.csproj             # Test project
│   │
│   └── 📁 Modules/                       # Mirrors src/Modules
│       ├── 📁 Strava/
│       │   └── 📁 Features/
│       │       └── ImportActivityTests.cs
│       │
│       └── 📁 Users/
│           └── ...
│
└── 📁 docs/                              # (Optional) Additional docs
    └── [deployment, API examples, etc]
```

---

## 📍 Key Locations Quick Reference

### When You Need...

| What | Where | File |
|------|-------|------|
| **Project vision** | Root | `.specs/project/PROJECT.md` |
| **What to build next** | Root | `.specs/project/ROADMAP.md` |
| **Current blockers** | Root | `.specs/project/STATE.md` |
| **Code patterns** | Codebase docs | `.specs/codebase/CONVENTIONS.md` |
| **Tech stack** | Codebase docs | `.specs/codebase/STACK.md` |
| **Feature requirements** | Feature folder | `.specs/features/[feature]/spec.md` |
| **Strava import code** | Source | `src/Modules/Strava/` |
| **Strava import tests** | Tests | `tests/Modules/Strava/Features/ImportActivityTests.cs` |
| **Automation commands** | Root | `Makefile` |
| **Run the app** | Root | `make run` |
| **Run tests** | Root | `make harness` |

---

## 🎯 How Aider Navigates This Structure

### 1️⃣ When You Say "Implement Spec 001"
```bash
/add .specs/features/strava-import/spec.md
/add .specs/codebase/CONVENTIONS.md
```

Aider then:
- Reads spec.md → understands WHAT to build
- Reads CONVENTIONS.md → understands HOW to build it
- Creates files in `src/Modules/Strava/` following the pattern
- Creates tests in `tests/Modules/Strava/`
- Runs `make harness` to validate

### 2️⃣ When You Say "Add Google OAuth"
```bash
/add .specs/features/google-oauth/spec.md
/add .specs/codebase/CONVENTIONS.md
/add .specs/codebase/STACK.md (if new dependencies needed)
```

Aider then:
- Creates `src/Modules/Users/`
- Adds OAuth endpoint to Users module
- Integrates with existing DB
- Tests everything

### 3️⃣ Key Principle for Token Efficiency
**Only add files Aider actually needs**

```bash
# ❌ Too Much (wastes tokens)
/add .

# ❌ Also Too Much
/add src/
/add tests/
/add .specs/

# ✅ Just Right
/add .specs/features/[feature]/spec.md
/add .specs/codebase/CONVENTIONS.md
```

---

## 🏗️ Module Structure Explained

### Why Each Module Has Domain/Features/Infrastructure?

```
Strava/
├── Domain/               # WHAT the system knows
│   ├── StravaActivity    # Entity with business rules
│   └── IStravaRepository # Interface (abstraction)
│
├── Features/             # HOW users interact
│   └── ImportActivity/   # One use case
│       ├── Endpoint      # HTTP interface
│       ├── Handler       # Business logic
│       ├── Request DTO   # Input contract
│       └── Response DTO  # Output contract
│
└── Infrastructure/       # HOW it persists
    └── StravaRepository  # Implements IStravaRepository
```

**Why this 3-part split?**
1. **Domain**: Reusable business rules (can be tested without DB/HTTP)
2. **Features**: Use cases (clear entry points, easy for Aider to find)
3. **Infrastructure**: Implementation details (easy to swap/test)

**Result**: Cohesion is HIGH (all Strava code in one place), dependencies are CLEAR, tests are SIMPLE.

---

## 🧪 Test Organization Mirrors Source

**Critical**: Tests folder structure **exactly mirrors** src structure.

```
src/Modules/Strava/Features/ImportActivity/
        ↓ (test mirrors)
tests/Modules/Strava/Features/ImportActivityTests.cs
```

**Why?** Aider can deduce where tests go. Without this mirroring, Aider creates test files in wrong places.

---

## 📊 File Sizes (For Context Management)

| File | Size | Tokens | Notes |
|------|------|--------|-------|
| PROJECT.md | ~2 KB | 400 | Always load |
| ROADMAP.md | ~2 KB | 400 | Load when planning |
| STATE.md | ~3 KB | 500 | Always load (memory) |
| STACK.md | ~3 KB | 500 | Load when architecture questions |
| CONVENTIONS.md | ~4 KB | 700 | Always load (patterns) |
| spec.md (per feature) | ~2-4 KB | 300-600 | Load when working feature |
| **Base Load Total** | ~10 KB | ~2000 | Project setup |
| **Per Feature** | +2-4 KB | +300-600 | When implementing |

**Target**: Keep main context under 15k tokens (leaves 185k for actual work)

---

## 🔄 When to Create New Modules

### Criteria for a New Module:
1. **Owns its own Domain**: Has entities, business rules
2. **Has Multiple Features**: > 1 use case
3. **Integrates with Database**: Needs repository pattern
4. **Independent**: Minimal coupling to other modules

### Example: When to Create Users Module
✅ **YES** if:
- Has User entity
- Has 2+ features (Login, Profile, Logout)
- Stores in database
- Loosely couples with Strava module (via FK only)

### Example: When NOT to Create Module
❌ **NO** if:
- Just utility functions (put in `src/Shared/`)
- Only one endpoint (put in existing module)
- No database (just configuration)

---

## 🚀 Scaling Rules (If You Grow)

### If 1-3 Modules
**Keep**: Current structure (Monolito works)

### If 4-6 Modules
**Consider**: Splitting into separate projects IF modules are truly independent
```
Monolito becomes:
├── SprintUp.Core (shared)
├── SprintUp.Strava
├── SprintUp.Users
├── SprintUp.Workouts
└── SprintUp.API (orchestration)
```

### If >6 Modules
**Migrate**: To microservices (each module = separate service)
```
Services:
├── strava-service
├── users-service
├── workouts-service
├── api-gateway
└── data-lake
```

**Note**: You're NOWHERE near this yet. MVP is monolito. Decide at 10k+ users.

---

## ✅ Things to Know

### What Goes Where
- **Domain logic** → `Modules/[Module]/Domain/`
- **HTTP endpoints** → `Modules/[Module]/Features/[Feature]/Endpoint.cs`
- **Feature logic** → `Modules/[Module]/Features/[Feature]/Handler.cs`
- **Database access** → `Modules/[Module]/Infrastructure/`
- **Shared utilities** → `src/Shared/` (create if needed)
- **Configuration** → `Program.cs` (centralized)
- **Database schema** → `SprintUpDbContext.cs`

### What NOT to Do
- ❌ Don't create `Utils/` folder with random helpers
- ❌ Don't create per-module DbContext
- ❌ Don't put repository interfaces in Infrastructure (put in Domain)
- ❌ Don't create DTOs outside Features
- ❌ Don't share Handlers across features

---

## 📋 Checklist When Adding a New Feature

- [ ] Created `.specs/features/[feature]/spec.md`
- [ ] Created `src/Modules/[Module]/Features/[Feature]/` folder
- [ ] Created endpoint, handler, DTOs in Features folder
- [ ] Created domain model in `Domain/` if new entity
- [ ] Created repository interface in `Domain/` if new persistence
- [ ] Created repository implementation in `Infrastructure/`
- [ ] Created tests in `tests/Modules/[Module]/Features/`
- [ ] Tests mirror source folder structure exactly
- [ ] `make harness` passes
- [ ] Git commit with atomic message

---

**Version**: 1.0  
**Last Updated**: May 15, 2026  
**Maintained By**: Fabio