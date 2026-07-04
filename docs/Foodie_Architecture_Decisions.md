# Foodie — Architecture Decisions

**Scope:** v1/MVP system architecture
**Status:** Accepted — Sprint 0

---

## 1. Three-Tier Architecture

Foodie is split into three tiers, each a separate process, with internal layering inside the middle tier:

```
Client Tier       →  React + Vite (SPA)
                      ├── Login / Register pages ............. Sprint 0
                      ├── Inventory page, Add/Edit/Delete ..... Sprint 1
                      ├── Locations UI, Expiry dashboard,
                      │     filtering ......................... Sprint 2
                      └── Dashboard widgets, Search,
                            loading/empty/error states ........ Sprint 3
        │
        ▼
App Logic Tier     →  ASP.NET Core Web API
                      (Controllers → Services → Repositories, one process)

  Controllers      →  HTTP in/out
                      ├── (no AuthController — Supabase Auth
                      │     handles register/login directly
                      │     from the client) .................. Sprint 0
                      ├── FoodItemsController ................. Sprint 1
                      ├── LocationsController, expiry
                      │     endpoints ......................... Sprint 2
                      └── Consume endpoint, search/filter,
                            global exception handling ......... Sprint 3
        │
        ▼
  Services         →  Business rules
                      ├── JWT validation config only (no
                      │     issuing, no password hashing —
                      │     Supabase Auth owns both) .......... Sprint 0
                      ├── Item validation rules ................ Sprint 1
                      ├── Expiry calculation, location-
                      │     assignment rules .................. Sprint 2
                      └── Consumption logic, dashboard
                            aggregation ....................... Sprint 3
        │
        ▼
  Repositories     →  EF Core data access
                      ├── (no UserRepository — no public.Users
                      │     table; Supabase owns auth.users) .. Sprint 0
                      ├── FoodItemRepository ................... Sprint 1
                      ├── LocationRepository, expiry-filtered
                      │     queries ............................ Sprint 2
                      └── Consumption updates, dashboard
                            summary queries .................... Sprint 3
        │
        ▼
Database Tier      →  PostgreSQL (Supabase-hosted)
                      ├── auth.users managed entirely by
                      │     Supabase Auth ...................... Sprint 0
                      ├── FoodItems table (UserId as UUID FK
                      │     into auth.users) ................... Sprint 1
                      ├── Locations table, FK to FoodItems ...... Sprint 2
                      └── Schema support for consumption ........ Sprint 3
```

Each arrow is a process boundary the previous tier can't see past — the client never talks to the database directly, and the database has no awareness of the client. Controllers, Services, and Repositories all run inside the *same* Tier 2 process; that's internal code organization, not three more tiers. The system is still 3-tier — Tier 2 just has structure inside it.

*One boundary is deliberately different: for register/login specifically, the Client Tier talks directly to Supabase Auth, not through Tier 2.* This isn't the client reaching into the Database Tier — Supabase Auth is a managed identity service sitting in front of the `auth.users` data, not raw table access. Every other operation (Locations, FoodItems, expiry, consumption) still flows Client → Tier 2 → Tier 3 exactly as before; Tier 2 just receives an already-issued JWT to validate rather than issuing one itself.

*Tier 1 renders client-side (CSR), not server-side — Foodie is a private, authenticated app with no anonymous-visitor or SEO audience, so SSR's main benefits don't apply. Revisit only if a public, SEO-relevant surface is added later.*

---

## 2. 3-Tier vs. Microservices

The client/server split already gives at least two tiers by default, so the real decision is whether Tier 2 should be one service or several:

- **Scalability:** Foodie's domains (inventory, locations, expiry) have near-identical, low load, so there's nothing to gain from scaling them separately.
- **Performance:** one service keeps every Controller → Service → Repository call in-process, with no added network latency between domains.
- **Resilience:** every feature depends on a user being logged in, so isolating inventory from auth into separate services wouldn't actually help — if auth is down, inventory is unusable either way. A single, well-tested service handles this fine at this scale.

**Decision:** 3-tier, one app-logic service, internally layered into Controllers → Services → Repositories.

---

## 3. Controllers → Services → Repositories: Why This Pattern

This is **Layered Architecture**, with the data-access layer following the **Repository pattern** — both are named, standard patterns, documented in Microsoft's own .NET architecture guidance and widely used across the ASP.NET Core community. Each layer has one reason to change and depends only on the layer below it: Controllers handle HTTP, Services hold business rules, Repositories handle data access.

**Why it matters for Foodie:** changing the database only touches Repositories; changing a business rule (e.g. consumption logic) only touches Services — neither risks breaking the other. Business rules can also be unit tested directly, without a database or HTTP request.

**Decision:** Layered Architecture inside Tier 2 — Controllers → Services → Repositories.

---

## 4. SQL vs. NoSQL

Foodie's domains (Users, Households, Food Items, Storage Locations, Categories, Consumption Logs) have stable, well-known foreign-key relationships — `FoodItem` belongs to `Location`, which belongs to `Household`, which has many `Users`. Common queries are join-heavy (e.g., "items expiring in 3 days, grouped by location, for this household"), which SQL handles natively. None of NoSQL's strengths (unstructured/highly variable data, very high write throughput, no need for joins) describe Foodie.

**Decision:** SQL (relational). Specific engine choice is covered in the tech stack document.

---

## Summary

| Decision | Outcome |
|---|---|
| Overall structure | 3-tier (Client / App Logic / Database) |
| Tier 1 rendering | Client-side rendering (CSR) — no SSR needed for a private, authenticated app |
| Tier 2 internal structure | Layered Architecture — Controllers → Services → Repositories, one service, no microservices |
| Database type (Tier 3) | Relational (SQL) — matches the foreign-key-heavy data shape |