# CONCERNS.md - Technical Constraints & Debt

## 🧠 1. Token Usage Optimization
- **Rule**: Do not feed multiple feature specifications to the AI context at the same time. Keep context focused strictly on the feature under development.

## 💾 2. Local Hardware Constraints (8GB Setup)
- **Constraint**: Running PostgreSQL via Docker alongside development tools requires lightweight memory usage.
- **Mitigation**: EF Core tracked instances must be short-lived. Databases must be cleared using transactional teardowns between testing suites.

## 🔒 3. LGPD Security Risk
- **Constraint**: Storing sensitive athlete metrics (weight, birth date) requires strict encryption at rest or isolated access filters.
- **Mitigation**: Never log authorization tokens, passwords, or personal health metrics to standard console outputs.