# STATE.md - Project Memory & Context

**Status**: Day 0 - SDD Setup & Requirements Gathering  

---

## 🎯 Current Status

| Item | Status | Details |
|------|--------|---------|
| **Project Setup** | 🔄 In Progress | Folder structure defined, writing initial specs |
| **Spec 001** | ⏳ Pending | Needs detailed markdown before coding |
| **Database** | ⏳ Pending | Awaiting Spec 001 schema definitions |

---

## ✅ Completed Decisions

### Architecture & Design
- **Google Onboarding Gate**: ✅ Google SSO registration will not collect weight/height during the initial OAuth handshake. Instead, a mandatory onboarding interceptor will force users to input weight and height immediately after their first successful Google login before any other app feature is unlocked.
- **Agnostic Integrations**: ✅ We will use an `IActivityProvider` interface. The system will NOT be hardcoded to Strava, even though Strava is the first implementation.
- **Security & LGPD**: ✅ Strict adherence. Users stay in a "Pending" state until email validation is complete. Consent must be tracked.
- **i18n Support**: ✅ Handled at the API level from Day 1. Endpoints will respect `Accept-Language` headers for PT-BR, EN, and ES.

### Tech Stack Choices
- **Emails**: ✅ Will use a free-tier transactional email provider (e.g., Resend or SendGrid) for validation links to keep costs at zero.

---

## ⚠️ Active Blockers

- 🟢 None currently. Ready to begin drafting Spec 001.

---

## 📋 TODOs

### Immediate
- [ ] Write `.specs/features/auth-registration/spec.md` (Spec 001).
  - Must include: Identity schema, Email sending interface, i18n error messages structure.
- [ ] Initialize standard `.NET 8` solution and minimal `Dockerfile`.

### Deferred
- [ ] Coach Profiles (Moved to V2 roadmap)
- [ ] AI Training Recalibration (Moved to V3 roadmap)