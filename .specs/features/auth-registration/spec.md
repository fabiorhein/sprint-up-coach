# Spec 001: Local User Registration, LGPD & Blazor UI

## 1. Objective
Implement the core `User` entity and the local registration workflow (email/password). This includes data persistence, password hashing, LGPD consent logging, dummy email validation triggering, and the responsive Blazor frontend.

## 2. Location
- **Module**: `UserManagement`
- **Feature**: `RegisterUser`
- **UI Route**: `/register`
- **API Route**: `POST /api/v1/auth/register` (for potential mobile clients)

## 3. Domain Model Rules (Entities & Enums)
Create the `User` entity and necessary Value Objects in the Domain layer:
- **Id**: UUID (Primary Key).
- **Email**: Unique index, required.
- **PasswordHash**: Required (Use ASP.NET Core Identity `PasswordHasher<User>`).
- **BirthDate**: DateOnly, required. Athlete must be >= 18 years old.
- **Gender**: Enum (Male, Female, NonBinary, PreferNotToSay).
- **WeightKg**: Decimal, required for local registration. Must be > 0.
- **HeightCm**: Integer, required for local registration. Must be > 0.
- **LgpdConsent**: Boolean, must be `true` to register. Log the timestamp of consent (`ConsentDateUtc`).
- **Status**: Enum `UserStatus` -> Default to `EmailValidationPending` upon creation.

## 4. UI/UX Rules (Blazor Web App)
- **Component**: `RegisterUserPage.razor` within the feature folder.
- **Styling**: strictly Bootstrap 5 utility classes. Mobile-first grid (`col-12 col-md-6` for form fields).
- **Validation**: Use `<EditForm>` and `<DataAnnotationsValidator>`.
- **i18n**: The form labels, placeholders, and validation error messages must support `Accept-Language` context (PT-BR, EN, ES). Default to PT-BR if missing.

## 5. Application Logic (The Handler)
- **Input**: `RegisterUserCommand` (DTO containing the raw form data).
- **Process**:
  1. Validate if the email already exists in `SprintUpDbContext`. If yes, return a standard RFC 7231 ProblemDetails error.
  2. Hash the raw password.
  3. Create the `User` entity with `EmailValidationPending` status.
  4. Save to the database.
  5. Fire the `IEmailService.SendValidationEmailAsync(User)` interface method.
- **Output**: Return the created User's UUID and a success message.

## 6. Infrastructure
- Create `SprintUpDbContext` in `src/Shared/Infrastructure/` (if it doesn't exist) and map the `User` entity using EF Core Fluent API.
- Create a dummy implementation of `IEmailService` called `ConsoleEmailService` that just uses `ILogger` to output the validation link to the terminal. Register it in `Program.cs`.

## 7. Acceptance Criteria (Harness Tests)
The AI must write the following tests in `tests/Modules/UserManagement/Features/RegisterUser/` before completing the task:
1. **Domain Test (Unit)**: Ensure `User` entity throws an exception if `BirthDate` calculates to under 18 years old.
2. **Handler Test (Integration)**: Use an in-memory SQLite or a clean Postgres schema to verify that passing valid data successfully persists the user and calls the `IEmailService` mock exactly once.
3. **UI Test (bUnit)**: Verify that the `RegisterUserPage.razor` renders a submit button and that clicking it with an empty form displays validation errors.

## 8. Execution Command
After writing the code, the agent MUST run:
`make harness`
Do not consider this spec done until the build and all tests pass cleanly.