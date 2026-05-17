# ROADMAP.md - SprintUp Feature Roadmap

**Last Updated**: May 2026  
**Release Cycle**: 1 feature per week (ideally)  
**Status Tracking**: See STATE.md for current progress

---

## 📊 Roadmap Overview

```
Sprint 1 (Week 1-2): MVP Core
├── Spec 001 ✅ Strava Activity Import
├── Spec 002 🔄 Google OAuth Login
└── Spec 003 ⏳ Basic Dashboard

Sprint 2 (Week 3-4): Intelligence
├── Spec 004 ⏳ Metrics (Pace/Speed/Volume)
├── Spec 005 ⏳ Weekly Training Load
└── Spec 006 ⏳ Training Alerts

Sprint 3 (Week 5-6): AI Features
├── Spec 007 ⏳ Training Recommendations
├── Spec 008 ⏳ Injury Risk Scoring
└── Spec 009 ⏳ Form Analysis

Sprint 4 (Week 7-8): Polish
├── Spec 010 ⏳ Mobile Responsiveness
├── Spec 011 ⏳ Analytics Dashboard
└── Spec 012 ⏳ Data Export
```

---

## 🎯 Sprint 1: MVP Core (Current Sprint)

**Goal**: Get Strava integration + user auth + basic dashboard  
**Timeline**: 2 weeks  
**Status**: In Progress

### Spec 001: Strava Activity Import ✅
- **Objective**: Receive Strava webhook, parse activity, save to DB
- **Complexity**: Medium
- **Files Changed**: ~6
- **Estimated Time**: 3 hours
- **Status**: 🟢 DONE
- **Path**: `.specs/features/strava-import/spec.md`

**What it delivers**:
- POST `/api/v1/activities/import` endpoint
- StravaActivity entity with Pace/Speed calculation
- PostgreSQL persistence
- 8 xUnit tests

**Dependencies**: None (first feature)

---

### Spec 002: Google OAuth Login 🔄
- **Objective**: Authenticate users via Google, create user session
- **Complexity**: Medium-High
- **Files Changed**: ~8
- **Estimated Time**: 4 hours
- **Status**: 🟡 READY FOR SPEC
- **Path**: `.specs/features/google-oauth/spec.md` (TODO: Write)

**What it delivers**:
- GET `/api/v1/auth/google` (redirect to Google)
- POST `/api/v1/auth/callback` (handle OAuth response)
- User entity with email, external ID
- JWT token generation & validation
- 10 xUnit tests

**Dependencies**: None (independent)

**Blockers**: 
- Need Google OAuth credentials setup (manual step)

---

### Spec 003: Basic Dashboard ⏳
- **Objective**: REST endpoint returning user's activities + basic stats
- **Complexity**: Low-Medium
- **Files Changed**: ~4
- **Estimated Time**: 2 hours
- **Status**: 🔵 BLOCKED (depends on Spec 001 & 002)
- **Path**: `.specs/features/dashboard/spec.md` (TODO: Write)

**What it delivers**:
- GET `/api/v1/dashboard` → user stats
- Total distance, total time, avg pace, longest run
- Last 10 activities list
- 6 xUnit tests

**Dependencies**: 
- ✅ Spec 001 (activities in DB)
- ✅ Spec 002 (user auth)

---

## 🎯 Sprint 2: Intelligence (Planned)

**Goal**: Add advanced metrics and training insights  
**Timeline**: Weeks 3-4  
**Status**: 🔵 Blocked (waiting for Sprint 1 completion)

### Spec 004: Detailed Metrics ⏳
**Objective**: Calculate pace zones, cadence, elevation metrics per activity  
**Complexity**: Medium  
**Dependencies**: Spec 001 ✅

### Spec 005: Weekly Training Load ⏳
**Objective**: Aggregate metrics over week, show volume trend  
**Complexity**: Medium  
**Dependencies**: Spec 004

### Spec 006: Smart Alerts ⏳
**Objective**: Notify user of overtraining, missing long run, etc.  
**Complexity**: Medium-High  
**Dependencies**: Spec 005

---

## 🎯 Sprint 3: AI Features (Planned)

**Goal**: Integrate AI for personalized recommendations  
**Timeline**: Weeks 5-6  
**Status**: 🔵 Blocked (waiting for Sprint 2)

### Spec 007: Training Recommendations ⏳
**Objective**: AI suggests next week's workouts based on history  
**Complexity**: High (new domain - ML)  
**Dependencies**: Spec 005, Spec 006

### Spec 008: Injury Risk Scoring ⏳
**Objective**: Score risk of injury based on volume, intensity progression  
**Complexity**: High  
**Dependencies**: Spec 005

### Spec 009: Form Analysis ⏳
**Objective**: Detect form/pace anomalies in recent runs  
**Complexity**: Medium  
**Dependencies**: Spec 001, Spec 004

---

## 🎯 Sprint 4: Polish (Planned)

**Goal**: Production readiness, UX polish, analytics  
**Timeline**: Weeks 7-8  
**Status**: 🔵 Blocked

### Spec 010: Mobile Responsiveness ⏳
### Spec 011: Analytics Dashboard ⏳
### Spec 012: Data Export (CSV/JSON) ⏳

---

## 📌 Blocked Features (Future)

These are ideas that are **deferred** (not in MVP):

### Rate Limiting (V2)
- Spec: Limit API calls per user/IP
- Why deferred: Not needed for MVP (<100 users)
- When to implement: Post-MVP monitoring

### Machine Learning Models (V2)
- Spec: Predictive injury scoring
- Why deferred: Requires data collection (3+ months)
- When to implement: After 500+ users

### Mobile App (V3)
- Spec: Native iOS/Android
- Why deferred: Web MVP first
- When to implement: After web validation

### Team/Coach Mode (V3)
- Spec: Coach can view athlete profiles
- Why deferred: Single-user focus first
- When to implement: Enterprise feature

---

## 🔄 Milestone Definitions

| Milestone | Specs | Status | ETA |
|-----------|-------|--------|-----|
| **MVP Ready** | 001, 002, 003 | 🟡 In Progress | Week 2 |
| **Smart Ready** | 004, 005, 006 | 🔵 Blocked | Week 4 |
| **AI Ready** | 007, 008, 009 | 🔵 Blocked | Week 6 |
| **Production Ready** | 010, 011, 012 | 🔵 Blocked | Week 8 |

---

## 📊 Metrics Tracking

### Velocity (Features Per Week)
- **Week 1**: Spec 001 ✅ (1 feature, 3 hours)
- **Week 2**: Spec 002 (waiting to start)
- Target: 1.5 features/week

### Token Efficiency (Per Feature)
- **Spec 001**: 2.5k tokens (under estimate of 5k) ✅
- **Target**: < 5k per feature

### Test Coverage
- **Spec 001**: 8 tests (100% of requirements) ✅
- **Target**: > 80% coverage

---

## 🎯 Next Actions

1. ✅ Spec 001 complete
2. 🔄 Write Spec 002 (Google OAuth)
3. ⏳ Start Spec 002 implementation
4. ⏳ Write Spec 003 after 001 & 002 done
5. ⏳ Reassess roadmap after Sprint 1

---

## 📝 Decision Log

### Q: Why no "Settings" feature in MVP?
**A**: Users don't need it. They can't change much until auth + data exist.

### Q: Why OAuth before email/password?
**A**: OAuth is simpler (less code, no password hashing to manage early).

### Q: Why no real-time WebSockets?
**A**: Not needed. Polling is fine for MVP scale.

### Q: Why not start with mobile?
**A**: Web first validates the idea. Mobile can follow.

---

**Last Updated**: May 2026  
**Next Review**: After Spec 003  
**Maintained By**: Fabio