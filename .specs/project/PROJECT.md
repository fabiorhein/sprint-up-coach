# PROJECT.md - SprintUp Coach

**Project**: SprintUp Coach - Running Training Intelligence Platform  
**Vision**: Empower runners with AI-driven training insights via agnostic app integrations, starting with Strava.  
**Status**: MVP V1 - Planning & SDD Setup  
**Owner**: Fabio  

---

## 🎯 Vision & Goals

### Primary Goal
Build a secure, LGPD-compliant, lightweight SaaS that integrates training data, allows users to set goals, and provides automated AI analysis of their performance.

### Core Premises (Non-Negotiable)
1. **LGPD Compliance**: Strict adherence to data privacy, consent management, and right-to-be-forgotten.
2. **Security-First**: Robust protection against vulnerabilities, secure credential storage, and safe OAuth flows.
3. **Responsive Design**: Mobile-first web approach; accessible via smartphone and desktop seamlessly.
4. **Internationalization (i18n)**: Native support for PT-BR, EN, and ES based on browser locale or user preference.

---

## 🛠️ Tech Stack & Architecture

### Backend
- **Runtime**: .NET 8 (Minimal APIs)
- **Database**: PostgreSQL 16
- **Architecture**: Modular Monolith (DDD Tactic)
- **Security**: ASP.NET Core Identity, JWT, HTTPS enforced
- **Data Ingestion**: Agnostic `IActivityProvider` interface (Provider Pattern)

### AI & Automation
- **Agent**: Aider (terminal-based) via Gemini 2.0 Flash
- **AI Analysis**: Post-activity LLM evaluation against user goals

---

## 🌍 Strategic Design (Bounded Contexts)

#### 1. User Management Context 👤
- **Owns**: Auth (Local with email validation OR Google SSO). Profiles (Demographics, Height, Weight), i18n preferences, LGPD consent.
- **Onboarding Gate**: Users registering via Google SSO who lack mandatory metrics (weight/height) are flagged as `OnboardingPending`. The API strictly blocks access to features until this data is supplied post-first-login.
- **Concepts**: User, Credentials, OnboardingStatus (Active, OnboardingPending, EmailValidationPending), Demographics, Consent.

#### 2. Integration Context 🏃 (Agnostic Data Ingestion)
- **Owns**: OAuth with third-party apps, Webhooks, Data syncing (Daily/Manual).
- **Implementations**: Strava (Initial).
- **Concepts**: IntegrationToken, ActivitySync, SyncLog.

#### 3. Training Analytics Context 📊
- **Owns**: User Goals (Distance, Speed, Performance, Timeframe), Activity standardization, Dashboards (Evolution, Benchmarks).
- **Concepts**: Activity, AthleteGoal, EvolutionMetric.

#### 4. AI Coaching Context 🤖
- **Owns**: Post-activity AI evaluation, Goal progression checking.
- **Concepts**: ActivityAnalysis, AIInsight.

---

## 🚀 The Future (V2 & V3 - Out of MVP Scope)
- **Coach Profiles**: Allow professionals to create and monitor team workouts.
- **RAG Architecture**: LLM augmented with professional training data and coach knowledge.
- **Dynamic Recalibration**: AI agent automatically adjusting training plans based on injuries or performance deviations.
- **Team Leaderboards**: Comparative metrics between athletes under the same coach.