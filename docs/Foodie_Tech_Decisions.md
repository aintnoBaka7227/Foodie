# Foodie — Tech Decisions

---

## 1. C# / ASP.NET Core 10

**Chosen over:** Node.js/Express, Django, Spring Boot

**Reason:** Native DI, validation, JWT middleware, and OpenAPI come built-in. Static typing catches bugs at compile time. First-class EF Core + PostgreSQL integration.

**Comparison:**
- **Vs. Node/Express:** Express runs on Node's single thread, so one slow synchronous operation blocks every other request. Kestrel dispatches each request to the thread pool, avoiding that bottleneck.
- **Vs. Django:** Django's async support was retrofitted onto an originally synchronous ORM/request lifecycle. ASP.NET Core's async/await is native through the whole pipeline — less friction for an I/O-bound JSON API.
- **Vs. Spring Boot:** Spring's DI needs explicit wiring (`@Autowired`/XML). ASP.NET Core's container resolves constructor-injected dependencies in one line per service — less boilerplate for the Controllers→Services→Repositories structure.

---

## 2. React 19 + Vite 8

**Chosen over:** Next.js, Create React App, Vue

**Reason:** UI is repeatable CRUD/dashboard components — React's component model fits directly. Vite gives a fast dev loop with no SSR overhead Foodie doesn't need.

**Comparison:**
- **Vs. Next.js:** SSR's value is fast, indexable HTML for anonymous visitors and search engines. Every Foodie screen is behind login — nothing to index, no anonymous visitor to optimize for.
- **Vs. Create React App:** no longer actively maintained; inherits a slower build pipeline with no offsetting benefit.
- **Vs. Vue:** React's larger ecosystem has deeper, better-maintained libraries for two planned features — barcode scanning (v2) and analytics charting (v5).

---

## 3. PostgreSQL 18

**Chosen over:** MongoDB, SQL Server, SQLite (long-term)

**Reason:** Core entities (FoodItem → Location → Household → User) are foreign-key-heavy with join-heavy queries — relational is the natural fit.

**Comparison:**
- **Vs. MongoDB:** "items expiring in 3 days, by location, by household" is one join in Postgres; in MongoDB it needs `$lookup` stages or denormalized data, pushing referential integrity into app code.
- **Vs. SQL Server:** equal performance at this scale, but Postgres is fully open-source with no licensing tier, and has free-tier managed hosting options (e.g. Supabase) that SQL Server doesn't.
- **Vs. SQLite:** locks the whole file on write, so concurrent household members updating inventory would block each other. Postgres handles concurrent writes via MVCC.

---

## 4. Entity Framework Core 10

**Chosen over:** Dapper, raw ADO.NET

**Reason:** First-class Postgres + ASP.NET Core integration, built-in migrations for the evolving Sprint 0–3 schema, LINQ keeps queries statically typed.

**Comparison:**
- **Vs. Dapper:** faster for hand-tuned queries, but no migrations — every schema change across sprints would be hand-written. Foodie's bottleneck is schema churn, not query latency.
- **Vs. raw ADO.NET:** manual SQL, parameter binding, and row-mapping for every query — pure boilerplate EF Core's LINQ and change tracking already handle.

---

## 5. Supabase Auth (JWT issued by Supabase, validated by ASP.NET Core)

**Chosen over:** Self-issued JWT (original decision), Session-based auth

**Reason:** Stateless JWT still fits an SPA calling a separate API — no session store, claims avoid a DB round-trip per request. Supabase's Auth service (GoTrue) now owns registration, login, hashing, and token issuance; ASP.NET Core just validates via AddJwtBearer against Supabase's JWKS. No AuthController, no hashing code, no Users table — auth.users replaces it. Email/password only for v1; OAuth is a dashboard toggle later, no backend change needed.

**Comparison:**


**Vs. self-issued JWT (original decision):** less code to maintain, no hashing load on the API thread, token issuance decoupled from the API's own deploys/cold-starts. Trade-off: dependency on Supabase's uptime, and UserId moves from INT to UUID (Section 2 of Foodie_Database_Decisions.md).
**Vs. session-based:** unchanged — still needs a session store per request; JWT just verifies a signature.

---

## 6. Docker / Docker Compose

**Chosen over:** Manual local installs

**Reason:** Reproducible local Postgres without native installs or version drift. Matches the containerized shape used in Azure production.

**Comparison:**
- **Vs. manual installs:** a native install ties your DB version to one machine; reinstalling or onboarding a second machine means redoing setup. `docker-compose.yml` reproduces the same environment anywhere Docker runs.

---

## 7. Playwright

**Chosen over:** Cypress, Selenium

**Reason:** Single tool for cross-browser E2E (Chromium, Firefox, WebKit); auto-waiting reduces flaky tests.

**Comparison:**
- **Vs. Cypress:** runs inside the browser's own context, historically weaker for true WebKit/multi-tab support. Playwright drives browsers externally, with native support for all three.
- **Vs. Selenium:** WebDriver needs explicit waits for every interaction. Playwright auto-waits for elements to be actionable.

---

## 8. Deployment: Azure (App + Frontend) + Supabase (Database)

**Chosen over:** Render (free Postgres), Railway

**Reason:** This project prioritizes cost efficiency, trading off scalability headroom for lower cost and less operational overhead. Azure App Service Free tier (backend) and Static Web Apps Free tier (frontend) cover compute at no cost. Azure DB for PostgreSQL has no free tier — billed hourly from the first hour — so the database is hosted on **Supabase** instead, which is Postgres under the hood (no change to EF Core/Npgsql) with a genuinely free, indefinite tier.

**Comparison:**
- **Vs. Render's free Postgres:** Render deletes free database instances after 30 days; Supabase's free tier pauses on 7 days of inactivity but never deletes data. For a project worked on across sprints with gaps between sessions, pause-and-resume is materially safer than scheduled deletion.
- **Vs. Railway:** Railway's free allowance is a usage credit that runs out, not an indefinite free tier — less predictable for a long-running project than Supabase's flat free limits.

**Trade-off accepted:** App Service Free tier has no "Always On," so the backend can cold-start after idling — worth a warm-up request before a live demo or interview walkthrough.

---

## Coding Conventions

- **C# style:** PascalCase for classes/methods/properties, camelCase for locals/parameters. Nullable reference types enabled.
- **Structure:** Controllers → Services → Repositories, one folder per layer.
- **Naming:** Entities singular (`FoodItem`); DbSets plural (`FoodItems`).
- **Async:** All EF Core calls and controller actions are async.
- **Validation:** Data annotations for basic checks; business rules live in Services.
- **Secrets:** Env vars / gitignored `appsettings.Development.json` — never hardcoded.
- **Errors:** Global exception middleware; explicit try/catch around external calls.
- **Git:** Conventional commits (`feat:`, `fix:`, `docs:`, `refactor:`). Branches: `main`, `dev`, `feature/xxx`.
