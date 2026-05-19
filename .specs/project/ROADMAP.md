# ROADMAP.md - SprintUp Coach Feature Roadmap

**Status**: Planning Phase  
**Strategy**: MVP features first. Agnostic architecture from Day 1.

---

## 🎯 Sprint 1: Identity & Core Experience

### Spec 001: Multi-language User Registration & Email Validation 
- **Objective**: Create user entity and local registration workflow.
- **Mandatory Fields (Local)**: Email, password, birth date, gender, weight, height.
- **Features**: i18n support (PT-BR, EN, ES), LGPD consent logging, and local activation link.

### Spec 002: Google Sign-On & Mandatory Onboarding Gate
- **Objective**: Implement Google OAuth authentication. 
- **Workflow**: If the user is logging in for the first time via Google, create a shell profile with `OnboardingPending` status. Enforce a mandatory post-login configuration step to collect weight and height before unlocking the rest of the application.

---

## 🎯 Sprint 2: Goals & Agnostic Ingestion

### Spec 003: Athlete Goals Setup
- **Objective**: Allow validated users to set goals based on Distance, Speed, Performance, and Target Timeframe.

### Spec 004: Agnostic Strava Integration & Sync
- **Objective**: Implement Strava OAuth (read permission). Create daily cron job and manual trigger for syncing activities.
- **Features**: Notify user upon sync completion; graceful handling of empty histories.

---

## 🎯 Sprint 3: Intelligence & Visualization

### Spec 005: Post-Activity AI Analysis Agent
- **Objective**: Trigger an AI analysis after every new ingested activity to evaluate if the athlete is meeting their defined goals.

### Spec 006: Evolution Dashboards
- **Objective**: Create visual endpoints for athlete evolution over time, basic benchmarking, and goal progress.

---

## 📌 Deferred Features (V2/V3)

- **Coach/Athlete Hierarchy**: Profiles for professionals to manage students.
- **RAG for Training**: Injecting external running science into the LLM.
- **AI Recalibration**: Automatic adjustments for injuries or performance spikes.
- **Group Dashboards**: Leaderboards and team comparatives.