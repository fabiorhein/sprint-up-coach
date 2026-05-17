# STATE.md - Project Memory & Context

**Purpose**: Persistent memory across work sessions  
**Last Updated**: May 15, 2026  
**Next Review**: After Spec 002 completion

This file captures decisions, blockers, lessons, and context so you can resume work without re-reading everything.

---

## 🎯 Current Status

| Item | Status | Details |
|------|--------|---------|
| **Project Setup** | ✅ Complete | Folder structure, Makefile, Docker setup done |
| **Spec 001** | ✅ Complete | Strava import endpoint tested, committed |
| **Spec 002** | 🔄 Ready | Google OAuth spec ready to write |
| **First Deploy** | ⏳ Pending | After Spec 003 (dashboard) |

---

## ✅ Completed Decisions

### Architecture Decisions
- **Monolito Modular**: ✅ 1 .NET project, N modules (not microservices)
  - Reason: Aider navigates faster, fewer tokens, simpler for small team
  - Trade-off: Less flexible scaling later (OK for MVP)
  - Lock-in: LOW (can split to microservices later if needed)

- **No Clean Architecture**: ✅ Skip the extra layers (Core, Infrastructure, API)
  - Reason: Overhead not justified for single module
  - Trade-off: Less testable in some areas (acceptable)
  - Evidence: Domain models and features are testable enough

- **Minimal APIs**: ✅ Use .NET 8 Minimal APIs (not Controllers)
  - Reason: Less boilerplate, easier for Aider to work with
  - Lock-in: Can migrate to Controllers later if needed

- **Single DbContext**: ✅ One SprintUpDbContext for all entities
  - Reason: Aider doesn't need to search multiple files
  - Trade-off: Less modularity (OK for MVP)
  - Refactor point: Can create per-module contexts later

- **DDD Tactic**: ✅ Rich domain models + Factory Methods
  - Reason: Encapsulates business logic (Pace calculation lives in StravaActivity)
  - Trade-off: Slightly more code in entities
  - Evidence: Spec 001 tests validate this pattern works

### Technology Decisions
- **Gemini 1.5 Flash**: ✅ Free tier for Aider
  - Token cost per feature: ~2.5k (under budget of 5k)
  - Speed: Fast enough for implementation
  - Fallback: Switch to Gemini Pro if Flash insufficient

- **PostgreSQL Local**: ✅ Docker Compose + PgAdmin
  - Reason: Development simplicity
  - Production: Will migrate to cloud DB later
  - Data backup: Git doesn't track DB, only schema (EF migrations)

- **xUnit + FluentAssertions**: ✅ Test framework
  - Reason: Clear assertion syntax, widely used in .NET
  - Coverage: Spec 001 has 8 tests (100% of requirements)

### Process Decisions
- **Spec-Driven Development**: ✅ Write spec before code
  - Evidence: Spec 001 was clearer to implement than traditional "create import endpoint"
  - Efficiency: Reduced clarification questions with Aider to 0
  - Time: Spec + Implementation + Validation = 35 minutes

- **Atomic Commits**: ✅ One feature = one git commit per spec
  - Reason: Easy to review, easy to revert
  - Format: `feat: implement strava activity import`

- **Harness = Gate**: ✅ No feature complete until `make harness` passes
  - Evidence: All Spec 001 tests pass, endpoint works
  - Trust: If tests pass, feature is production-ready

---

## ⚠️ Active Blockers

### None Currently 🟢
All blockers from initial planning have been resolved.

### Previous Blockers (Resolved)
- ❌ "How to organize specs?" → ✅ Solved with tlc-spec-driven structure
- ❌ "How to keep tokens low?" → ✅ Solved with Gemini Flash + smaller specs
- ❌ "Aider integration unclear?" → ✅ Solved with QUICKSTART.md

---

## 💡 Lessons Learned

### What Worked Well ✅

1. **Clear Specs = Fast Implementation**
   - Spec 001 included exact calculations (Pace formula), test criteria, HTTP responses
   - Aider implemented it correctly first try (no rewrites)
   - Savings: ~70% fewer tokens vs vague requests

2. **Monolito Modular is Simple**
   - All Spec 001 code fits in one coherent module (Strava)
   - No dependency hell, no circular imports
   - Easier to understand from cold start

3. **Harness-First Thinking**
   - Writing tests before implementation (TDD style) forced clear thinking
   - Tests caught edge cases (like `distance == 0`)
   - Confidence: If tests pass, feature works

4. **One Feature at a Time**
   - No confusion between Spec 001 and planning Spec 002
   - Mental load stayed low
   - Ready to context-switch easily

### What Needs Adjustment ⚠️

1. **Google OAuth is More Complex Than Expected**
   - Spec 002 will involve external API (Google), tokens, sessions
   - May need to introduce abstraction layer for credential handling
   - Plan: Write detailed design.md before coding

2. **Database Migrations Not Automated Yet**
   - Currently manual: `dotnet ef database update`
   - Plan: Add `make migrate` to standardize
   - Blocker: None (low priority for MVP)

3. **No Frontend Yet**
   - Endpoints work, but no UI to test
   - Plan: Add postman collection or curl examples
   - Impact: Low (backend-first approach)

### What We'd Do Differently Next Time

1. ✅ Start with tlc-spec-driven structure day 1 (learned from this)
2. ✅ Include example curl requests in spec (will do for Spec 002)
3. ⚠️ Test OAuth locally before implementation (will do for Spec 002)

---

## 📋 TODOs

### Immediate (This Week)
- [ ] Write Spec 002 (Google OAuth) spec.md
  - Estimate: 30 min
  - Include: OAuth flow diagram, token handling, user entity schema
- [ ] Test Spec 002 implementation with Aider
  - Estimate: 4 hours
  - Blocker: Need Google OAuth credentials from https://aistudio.google.com

### Soon (Next Week)
- [ ] Write Spec 003 (Dashboard) spec.md
  - Depends on: Spec 001 & 002 complete
  - Estimate: 20 min
- [ ] Implement Spec 003
  - Estimate: 2 hours
- [ ] Update ROADMAP.md with actual velocity data

### Before Production (Week 8)
- [ ] Set up CI/CD pipeline (GitHub Actions)
- [ ] Add integration test for Strava webhook signature validation
- [ ] Create user documentation (README for front-end devs)
- [ ] Add API rate limiting
- [ ] Secure Google OAuth credentials (use environment variables)

### Deferred (Post-MVP)
- [ ] Migrate from local PostgreSQL to AWS RDS
- [ ] Add Redis caching layer
- [ ] Implement real-time WebSockets
- [ ] Add machine learning for injury prediction
- [ ] Build mobile app

---

## 🚫 Deferred Ideas (Not Doing in MVP)

### Rate Limiting
- **Idea**: Limit API calls per user/hour
- **Why Deferred**: < 100 users, not needed yet
- **When to Implement**: After monitoring shows spike in traffic
- **Complexity**: Medium (Redis required)

### Authentication via Email/Password
- **Idea**: Let users sign up with email
- **Why Deferred**: OAuth simpler, more secure
- **When to Implement**: If users request it
- **Complexity**: High (password management, email verification)

### Real-time Dashboard Updates
- **Idea**: WebSocket feed for live activity updates
- **Why Deferred**: Polling sufficient for MVP
- **When to Implement**: > 1000 concurrent users
- **Complexity**: High (WebSocket infrastructure)

### Batch Data Imports
- **Idea**: Upload CSV file with activities
- **Why Deferred**: Strava sync covers 99% of use case
- **When to Implement**: Power user feature
- **Complexity**: Medium

### Coach/Team Features
- **Idea**: Coach can view multiple athletes
- **Why Deferred**: Single-user focus first
- **When to Implement**: Enterprise pivot
- **Complexity**: High

---

## 🔗 Key Files Locations

### Project-Level
- `.specs/project/PROJECT.md` - Vision, goals, stack
- `.specs/project/ROADMAP.md` - Feature planning
- `.specs/project/STATE.md` - This file (memory)

### Codebase Docs (Future)
- `.specs/codebase/STACK.md` - Tech details
- `.specs/codebase/ARCHITECTURE.md` - System design
- `.specs/codebase/CONVENTIONS.md` - Code patterns
- `.specs/codebase/TESTING.md` - Test patterns
- `.specs/codebase/CONCERNS.md` - Tech debt, risks

### Features
- `.specs/features/strava-import/spec.md` - Spec 001
- `.specs/features/google-oauth/spec.md` - Spec 002 (TODO)
- `.specs/features/dashboard/spec.md` - Spec 003 (TODO)

---

## 🧠 Context Loading Quick Reference

### When Starting Work
```bash
# Base context (always add)
/add .specs/project/PROJECT.md
/add .specs/project/ROADMAP.md
/add .specs/project/STATE.md

# Feature-specific (add when working on feature)
/add .specs/features/[feature]/spec.md
/add .specs/codebase/CONVENTIONS.md

# Total tokens: ~8-10k (efficient)
```

### When Context is Getting Large
- Remove old feature specs
- Keep only active spec
- Keep STATE.md (it's small)

---

## 📞 Notes for Future Fabio

### When Resuming Work:
1. Read this file (STATE.md) first - 2 min
2. Read ROADMAP.md to see what's next - 3 min
3. Open Aider and add relevant specs
4. Jump back into work

### If You Feel Lost:
- Check ROADMAP.md for what's planned
- Check STATE.md for recent decisions
- Check git log for what was done recently: `git log --oneline -20`

### If Architecture Feels Wrong:
- It's probably OK (it's simple by design)
- Check "Completed Decisions" section above
- If you want to change, document decision in "Active Decisions" below

---

## 📝 Session Log

### Session 1 (May 15, 2026)
**Duration**: 45 minutes  
**What**: Set up project structure, created specs for Spec 001, implemented with Aider  
**Outcome**: Spec 001 complete, all tests passing  
**Tokens Used**: ~2.5k (Gemini Flash)  
**Learnings**: Specs are super powerful, Aider works well with clear requirements  
**Next**: Spec 002 (Google OAuth)

### Session 2 (May 22, 2026)
**Duration**: TBD  
**What**: TBD  
**Outcome**: TBD  

---

## ✅ Checklist for Session Handoff

Before pausing work:
- [ ] Update this STATE.md with current status
- [ ] Git commit all changes: `git commit -m "chore: update STATE.md"`
- [ ] Note next action in TODOs section
- [ ] Close Aider session

On resuming work:
- [ ] Read STATE.md (2 min)
- [ ] Read ROADMAP.md (3 min)
- [ ] Run `git log --oneline -10` to see what happened
- [ ] Open Aider again
- [ ] Start with next TODO

---

**Version**: 1.0  
**Maintained By**: Fabio  
**Last Sync**: May 15, 2026 @ 14:30 UTC