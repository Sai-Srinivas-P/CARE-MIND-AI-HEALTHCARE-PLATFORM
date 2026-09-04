# CARE-MIND-AI-HEALTHCARE-PLATFORM

Portfolio-grade healthcare management API built with ASP.NET Core and C#.

> Educational project only. The AI assistant is not a doctor, does not diagnose, prescribe, or replace professional medical care.

## Stack
- .NET 10 / ASP.NET Core Web API
- C#
- Entity Framework Core 10
- SQL Server
- JWT bearer authentication
- OpenAPI
- Official OpenAI .NET SDK
- xUnit
- Docker Compose

## Features
- Patient registration and profiles
- Doctor management
- Appointment scheduling
- Medical records and medications
- Role-based authorization
- AI healthcare information assistant
- AI-generated patient visit-preparation summaries
- AI audit logging
- Safe AI fallback when no API key is configured
- Global exception handling
- Health checks
- EF Core migrations
- Seed/demo data
- Unit/API tests
- GitHub Actions CI

## Structure
```text
src/
  AIHealthCare.Api/
  AIHealthCare.Domain/
  AIHealthCare.Infrastructure/
tests/
  AIHealthCare.Api.Tests/
```

## Run

1. Install .NET 10 SDK and Docker Desktop.
2. Start SQL Server:

```bash
docker compose up -d sqlserver
```

3. Set your OpenAI key without committing it:

PowerShell:
```powershell
$env:OpenAI__ApiKey="your-api-key"
```

Optional:
```powershell
$env:OpenAI__Model="gpt-5.2"
```

4. Run:

```bash
dotnet restore
dotnet run --project src/AIHealthCare.Api
```

OpenAPI JSON:
`https://localhost:7001/openapi/v1.json`

## Demo login

All seeded demo accounts use the configured `Seed:DemoPassword`.

- admin@aihealthcare.local
- doctor@aihealthcare.local
- patient@aihealthcare.local

Default development password:
`ChangeMe123!`

Change it before any real deployment.

## API examples

### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@aihealthcare.local",
  "password": "ChangeMe123!"
}
```

### Ask AI
```http
POST /api/ai/ask
Authorization: Bearer <token>
Content-Type: application/json

{
  "question": "What questions should I prepare for a routine doctor visit?"
}
```

### Create appointment
```http
POST /api/appointments
Authorization: Bearer <token>
Content-Type: application/json

{
  "patientId": 1,
  "doctorId": 1,
  "scheduledAtUtc": "2026-10-01T10:00:00Z",
  "reason": "Routine follow-up"
}
```

## EF Core migrations

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project src/AIHealthCare.Infrastructure --startup-project src/AIHealthCare.Api --output-dir Data/Migrations
dotnet ef database update --project src/AIHealthCare.Infrastructure --startup-project src/AIHealthCare.Api
```

## Architecture

```text
Client
  |
  v
ASP.NET Core API
  |
  +-- Controllers / DTOs
  |
  +-- Services
  |
  +-- Domain
  |
  +-- Infrastructure
       +-- EF Core / SQL Server
       +-- OpenAI Responses API
```

## Production hardening checklist
- ASP.NET Core Identity or an external identity provider
- Refresh tokens and token rotation
- Secret storage such as a managed secret vault
- Encryption at rest and in transit
- Strong audit trails and access reviews
- Consent and data-retention controls
- BOLA/IDOR threat modeling
- Rate limiting
- Structured logging and tracing
- Redis caching where appropriate
- Background jobs for notifications
- Optimistic concurrency
- Healthcare/privacy compliance for the deployment jurisdiction
- AI evaluation, prompt-injection defenses, and human oversight

## Interview topics demonstrated
Dependency injection, middleware, JWT, claims/roles, EF Core, LINQ, async/await, DTOs, REST APIs, OpenAPI, configuration, exception handling, testing, Docker, CI/CD, and AI integration.
