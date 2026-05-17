# PROJECT.md - SprintUp

**Project**: SprintUp - Running Training Intelligence Platform  
**Vision**: Empower runners with AI-driven training insights via Strava integration  
**Status**: MVP V1 - Development  
**Owner**: Fabio  
**Created**: May 2026

---

## 🎯 Vision & Goals

### Primary Goal
Build a lightweight SaaS backend that integrates Strava data, calculates running metrics, and provides training intelligence without heavy infra.

### Secondary Goals
1. **Learn SDD in Practice** - Real-world project using Spec-Driven Development
2. **Minimize Tokens** - Prove efficient AI-assisted development with free APIs
3. **Zero Hardware Strain** - Run on 8GB Mac Mini, PostgreSQL in Docker
4. **Fast Iteration** - Ship meaningful features in 1-week sprints

---

## 📊 Success Metrics

| Metric | Target | Status |
|--------|--------|--------|
| **Time per Feature** | < 3 hours (spec → implemented → tested) | Tracking |
| **Token Cost** | < 5k per feature (OpenRouter free) | Tracking |
| **Test Coverage** | > 80% | TBD |
| **Spec Clarity** | 0 clarification questions needed | TBD |
| **Build Time** | < 10s (make harness) | TBD |

---

## 🛠️ Tech Stack

### Backend
- **Runtime**: .NET 8 (C# 12+)
- **Framework**: ASP.NET Core (Minimal APIs)
- **DB**: PostgreSQL 16
- **ORM**: Entity Framework Core (Fluent API)
- **Testing**: xUnit + FluentAssertions + Moq
- **VCS**: Git

### Architecture
- **Pattern**: Monolito Modular (1 project, N modules)
- **Design**: DDD Tático + SOLID
- **Code Style**: Clean Code (no Clean Architecture overhead)
- **Features**: Feature Folders (all related code together)

### Infrastructure
- **Containerization**: Docker Compose
- **Local DB**: PostgreSQL 16 + PgAdmin
- **CI/CD**: GitHub Actions (future)

### AI & Automation
- **Agent**: Aider (terminal-based)
- **LLM**: Gemini 2.0 Flash (via OpenRouter free tier)
- **Orchestration**: Make commands

---

## 🌍 Strategic Design (Stratigic DDD)

### Identified Bounded Contexts

SprintUp is organized into **4 independent business domains** (Bounded Contexts):

#### 1. Strava Integration Context 🏃
- **Owns**: Activity import, storage, metrics calculation (Pace, Speed, Distance)
- **Responsible for**: Real-time sync from Strava webhook
- **Does NOT own**: User management, training analysis, notifications
- **Core Concepts**: Activity, Athlete, Pace, Distance, Webhook
- **Status**: ✅ Implemented (Spec 001)

#### 2. User Management Context 👤
- **Owns**: Authentication, user profiles, sessions
- **Responsible for**: Google OAuth integration, credential management
- **Does NOT own**: Training data, activity storage
- **Core Concepts**: User, GoogleId, Email, Session, Credentials
- **Status**: 🔄 Planned (Spec 002)

#### 3. Training Analytics Context 📊
- **Owns**: Weekly metrics, training load calculation, performance insights
- **Responsible for**: Dashboard data, trend analysis
- **Does NOT own**: Raw activity data (Strava owns it), alerts (Notifications own it)
- **Core Concepts**: TrainingWeek, TrainingLoad, VolumeTrend, FatigueScore
- **Status**: ⏳ Planned (Spec 004+)

#### 4. Notifications Context 🔔
- **Owns**: Alert rules, delivery channels, alert templates
- **Responsible for**: Detecting and sending alerts
- **Does NOT own**: Metric calculation (Training Analytics owns it)
- **Core Concepts**: Alert, AlertRule, Channel, Trigger
- **Status**: ⏳ Planned (Spec 006+)

### Communication Between Contexts

**Pattern: Event-Driven (Async, Decoupled)**

```
Strava Integration
    ↓ publishes "ActivityImportedEvent"
    ├─→ Training Analytics (subscribes) → updates TrainingWeek
    ├─→ Notifications (subscribes) → checks alert rules
    └─→ User Management (already owns the user)

Training Analytics
    ↓ publishes "OvertrainingDetectedEvent"
    └─→ Notifications (subscribes) → sends Alert
```

**Benefit**: Contexts evolve independently. No tight coupling.

### Ubiquitous Language (Shared Vocabulary)

Each context uses precise terminology:

**Strava Integration**: Activity, Athlete, Pace, Distance, Webhook  
**User Management**: User, Session, Credentials, GoogleId  
**Training Analytics**: TrainingWeek, TrainingLoad, FatigueScore  
**Notifications**: Alert, AlertRule, Channel, Trigger  

**Why**: Prevents confusion, enables independent evolution.

### Strategic Design Decisions

**Decision 1**: Event-driven communication between contexts (not shared tables)  
**Decision 2**: One ubiquitous language per context (Activity ≠ TrainingActivity)  
**Decision 3**: No shared aggregates across contexts (each owns its data)  

**Future**: When 4+ contexts exist or team grows, consider splitting to microservices. Today: Monolito modular with clear boundaries.

---

## 🎭 Development Philosophy

### Spec-Driven Development (SDD)
1. Write spec before code
2. Spec includes business rules, validation, test criteria
3. AI implements from spec
4. Harness (tests) validates automatically
5. Commit on success

### Principles
- **Clarity First**: Ambiguity → explicit design decisions
- **Minimal Layers**: 1 project, 3-tier architecture (Domain/Features/Infrastructure)
- **Tests Are Gates**: No tests pass = feature incomplete
- **Token Efficiency**: Fewer files + clear specs = fewer tokens
- **One Feature at a Time**: No multi-threading, sequential execution

---

## 💰 Economics

| Cost Factor | Investment | ROI |
|-------------|-----------|-----|
| LLM API | $0 (OpenRouter free tier) | 10/10 |
| Database | $0 (local) | 10/10 |
| Framework | $0 (.NET free) | 10/10 |
| Hardware | Already owned | 10/10 |
| **Total Cost** | **$0** | **∞** |

---

## 📈 Roadmap (By Spec)

### Sprint 1: MVP Core
- **[Spec 001]** Strava Activity Import
- **[Spec 002]** Google OAuth Login
- **[Spec 003]** Basic Dashboard

### Sprint 2: Intelligence
- **[Spec 004]** Pace/Speed/Volume Metrics
- **[Spec 005]** Weekly Training Load
- **[Spec 006]** Simple Alerts

### Sprint 3: AI Features
- **[Spec 007]** Training Recommendations
- **[Spec 008]** Injury Risk Scoring
- **[Spec 009]** Form Analysis

### Sprint 4: Polish
- **[Spec 010]** Mobile Responsiveness
- **[Spec 011]** Analytics Dashboard
- **[Spec 012]** Export Data

---

## 🔄 Workflow Template (Per Feature)

```
SPECIFY
  ↓ (2-5 min)
  Define requirements, validation rules, test criteria
  Create: .specs/features/[feature]/spec.md
  
DESIGN [skip if straightforward]
  ↓ (if needed)
  Architecture decisions, component interactions
  Create: .specs/features/[feature]/design.md
  
TASKS [skip if <3 obvious steps]
  ↓ (if needed)
  Atomic task breakdown, dependencies
  Create: .specs/features/[feature]/tasks.md
  
EXECUTE
  ↓ (30-60 min)
  Aider implements, runs harness, commits
  Run: aider --model gemini/gemini-1.5-flash
  
VALIDATE
  ↓ (5-10 min)
  Manual testing, verify against spec, check git log
  Run: curl tests + git log
```

---

## 🎯 Constraints & Trade-offs

### What We Chose
- ✅ Monolito modular (not microservices)
- ✅ Free LLM (not Claude 3.5 Sonnet)
- ✅ Minimal APIs (not MVC controllers)
- ✅ EF Core direct (not Repository pattern)
- ✅ Single DbContext (not per-module)

### What We Avoided
- ❌ Multiple .csproj files
- ❌ Clean Architecture (too heavy)
- ❌ Design patterns overhead
- ❌ Enterprise patterns
- ❌ Over-engineering

### Why These Decisions?
**Goal: Minimize context window for Aider**

When Aider searches for a file to edit, it scans fewer directories → understands code faster → generates better → costs fewer tokens.

---

## 🧠 Key Insights

### Token Efficiency (From Early Experiments)
- **Bad**: "Create authentication module" → Aider invents 15 files → 8k tokens
- **Good**: "Implement Spec 002" + CONVENTIONS.md → Aider creates 4 files → 2.5k tokens
- **Savings**: ~70% with clear specs

### Time Efficiency (Per Feature)
- **Spec writing**: 10 min
- **Aider execution**: 20 min
- **Manual validation**: 5 min
- **Total**: ~35 min per feature

### Quality Signal
- If all tests pass → feature is production-ready
- If any test fails → Aider auto-retries
- If all retries fail → manual review needed (rare)

---

## 🚀 Getting Started

### First Time Setup
1. Clone/create repo
2. Copy .specs/ files
3. Run `docker-compose up -d`
4. Read README.md → QUICKSTART.md
5. Execute Spec 001 with Aider

### Daily Workflow
1. Pick next spec from ROADMAP.md
2. Open Aider: `aider --model gemini/gemini-1.5-flash`
3. `/add .specs/project/CONVENTIONS.md`
4. `/add .specs/features/[feature]/spec.md`
5. Request: "Implement [feature] spec"
6. Wait for `make harness` to pass ✅
7. Validate & commit

### Pausing/Resuming
1. Update STATE.md with status
2. Git commit: `git commit -m "chore: pause after Spec 003"`
3. On resume: Read STATE.md (2 min refresh)

---

## 📞 Context Loading Strategy

### Base Load (Always)
- PROJECT.md (this file)
- ROADMAP.md (feature planning)
- STATE.md (memory + blockers)
- **Total: ~4k tokens**

### Feature-Specific Load (On Demand)
- `.specs/features/[feature]/spec.md`
- `.specs/codebase/CONVENTIONS.md`
- `.specs/codebase/TESTING.md`
- **Total per feature: +5-8k tokens**

### Never Load Simultaneously
- Multiple feature specs
- Old archived specs
- Redundant docs

---

## ✅ Definition of Done (Per Feature)

A feature is complete when:

1. ✅ Spec written (with test criteria)
2. ✅ Code implemented (following CONVENTIONS.md)
3. ✅ All tests pass (`make harness`)
4. ✅ Manual validation done (curl test or manual click)
5. ✅ Git committed with atomic message
6. ✅ STATE.md updated

---

## 🔗 Related Documents

- **ROADMAP.md** - Feature list & milestones
- **STATE.md** - Current status, decisions, blockers
- **CONVENTIONS.md** - Code patterns
- **STACK.md** - Tech stack details
- **ARCHITECTURE.md** - System design

## 🤖 Your Aider Configuration

Your `.aider.conf.yml`:
```yaml
model: openrouter/google/gemini-2.0-flash-exp:free
weak-model: openrouter/google/gemini-2.0-flash-001
editor-model: openrouter/google/gemini-2.0-flash-001

dark-mode: true
pretty: true
stream: true
git: true
auto-commits: false
```

**Models Used**:
- **Main**: Gemini 2.0 Flash Exp (best performance)
- **Weak**: Gemini 2.0 Flash 001 (fallback)
- **Editor**: Gemini 2.0 Flash 001 (for editing)

**Cost**: $0 (OpenRouter free tier)  
**Speed**: Very fast  
**Quality**: Excellent for code generation

---

**Last Updated**: May 2026  
**Next Review**: After Spec 003  
**Maintainer**: Fabio