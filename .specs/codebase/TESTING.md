# TESTING.md - Test Patterns & Harness

## 🛠️ Testing Toolkit
- **Engine**: xUnit (isolated class execution).
- **Assertions**: FluentAssertions.
- **Mocks**: Moq for intercepting service or provider lifecycles.
- **UI Component Testing**: bUnit for rendering Blazor fragments in memory and checking HTML output.

---

## 🎯 Harness Strategy (Validation-First)
1. **Domain Isolation**: Domain entities must have unit tests covering math, logic, and state constraints without any mocks.
2. **UI Component Validation (bUnit)**: Blazor pages must have basic markup verification tests to guarantee that important user actions (like clicking a button or viewing validation errors) trigger the correct visual states.
3. **The Automated Gate**: Running `make harness` compiles the entire app and executes both the Backend logic tests and the bUnit UI tests. If any layout rendering or handler assertion fails, the build is rejected.