# Foodie — Tech Stack

*Versions verified 24 June 2026.*

| Layer | Technology | Why |
|---|---|---|
| Backend language | C# 13 (.NET 10) | Static typing catches bugs at compile time; pairs natively with EF Core |
| Backend framework | ASP.NET Core 10.0.9 (LTS) | Native DI, validation, JWT middleware, OpenAPI — no extra packages needed |
| SDK | .NET SDK 10.0.301 | Confirmed via `dotnet --list-sdks` |
| ORM | Entity Framework Core 10.0.9 | First-class Postgres + ASP.NET Core integration, built-in migrations |
| Frontend language | TypeScript 6.0.2 | Confirmed via installed `package.json` |
| Frontend framework | React 19.2.6 | Confirmed via installed `package.json` |
| Build tool | Vite 8.0.12 | Confirmed via installed `package.json` |
| Runtime (dev/CI) | Node.js 24.x — *exact patch not yet confirmed, run `node --version`* | Required by Vite 8; Active LTS line |
| Database | PostgreSQL 18.4 | Relational fit for foreign-key-heavy data; free, flexible, hosts anywhere |
| DB driver | Npgsql 10.0.x (EF Core provider) | Standard EF Core ↔ Postgres bridge |
| Auth | Supabase Auth (JWT) | Supabase issues/manages the JWT (registration, login, password hashing); ASP.NET Core validates it. Stateless, fits SPA-to-API calls without a session store |
| Auth client (frontend) | `@supabase/supabase-js` | Frontend SDK for register/login calls directly to Supabase Auth |
| E2E testing | Playwright 1.61.0 | Single tool, cross-browser, fits a React SPA |
| Backend testing | xUnit 2.9.x | Standard ASP.NET Core unit/integration testing |
| Containers | Docker — *not yet confirmed, run `docker --version`* | Reproducible local Postgres + dev/prod parity |
| Deployment | Azure (App Service/Container Apps + Static Web Apps + Azure DB for PostgreSQL) | Covers all three tiers under one provider |
| Coding agents | Claude Sonnet 4.6, ChatGPT 5.5 | Development assistance — not a runtime dependency, listed for context |