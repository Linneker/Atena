# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Atena** is an ERP (Enterprise Resource Planning) system for financial management — expenses, income, debt, payments, and cash flow. It consists of a .NET 7 REST API backend and multiple Angular frontends.

## Build & Run Commands

### Backend (.NET 7)

```powershell
# Build entire solution
dotnet build Atena.sln

# Run the API (from src/ERP/acme.atena.api/)
dotnet run --project src/ERP/acme.atena.api/acme.atena.api.csproj

# Run tests
dotnet test

# EF Core migrations
dotnet ef migrations add <MigrationName> --project src/ERP/acme.atena.infra
dotnet ef database update --project src/ERP/acme.atena.infra
```

### Frontend (Angular) — run from each app's directory

```powershell
# Install dependencies
npm install

# Development server (http://localhost:4200)
npm start

# Production build
npm run build

# Run tests
npm test

# Watch mode
npm run watch
```

Angular apps are under `site/`:
- `cashflow/` — Angular 13, primary cash flow dashboard
- `cashflow2/` — Angular 13 variant with Chart.js
- `coreui-free-angular-admin-template/` — Angular 14 admin panel

### Docker

```powershell
# Build API image (from src/ERP/acme.atena.api/)
docker build -t atena-api .

# Build MVC site image (from site/acme.sistemas.atena.mvc.site/)
docker build -t atena-site .
```

## Architecture

This follows a **Clean Architecture** pattern with strict layer boundaries:

```
acme.atena.api          → HTTP layer: controllers, DTOs, AutoMapper profiles, Swagger
acme.atena.application  → Business logic: application services, MediatR handlers
acme.atena.domain       → Core: entities, interfaces/contracts, domain DTOs
acme.atena.infra        → Infrastructure: EF Core DbContext, external config
acme.atena.repository   → Data access: repository implementations, validators
acme.atena.config       → DI container: all dependency injection wiring
acme.atena.core         → Cross-cutting: helpers, MediatR orchestration, messaging
```

Dependencies flow inward: `api → application → domain ← repository ← infra`. The `config` project registers all services and is referenced only by the API startup.

### Key Patterns

- **CQRS via MediatR**: Commands and queries live in `acme.atena.application/Handler/`; the core mediator is in `acme.atena.core/Mediador/`
- **Repository Pattern**: Data access is abstracted through interfaces defined in `acme.atena.domain/Interface/Service/` and implemented in `acme.atena.repository/`
- **AutoMapper**: Mapping profiles in `acme.atena.api/AutoMapper/` handle entity ↔ DTO conversions
- **OData**: API supports OData queries for flexible filtering/sorting on list endpoints
- **JWT Auth**: Token configuration in `appsettings.json`; middleware registered in `Startup.cs`

### Domain Areas

| Area | Entities |
|------|----------|
| Account | Despesa (Expense), Receita (Income), Divida (Debt), Pagamento (Payment), FluxoDeCaixa (Cash Flow) |
| Person | Empresa (Company), Fornecedor (Supplier), Pessoa (Contact) |
| Product | Compra (Purchase), pricing, inventory |
| Security | Authentication, JWT |

## Database

- **Primary**: MySQL via Pomelo.EntityFrameworkCore.MySql
- **Secondary**: SQL Server support also configured
- Connection strings are in `appsettings.json` (host: `bd.thor.hostazul.com.br:4406`)
- EF Core manages schema via migrations in `acme.atena.infra`

## API Documentation

Swagger UI is available at `/swagger` when running in Development mode. The API uses NLog for structured logging (configured in `src/ERP/acme.atena.api/NLog.config`).
