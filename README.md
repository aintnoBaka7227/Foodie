# Foodie

**A smart food inventory management system for university students.**

Foodie helps you track what food you actually have, where it's stored, and when it's about to go off — so less of it ends up in the bin.

---

## The Problem

University students living independently for the first time tend to lose track of what's in their fridge, pantry, and freezer. Food gets pushed to the back of a shelf, forgotten, and thrown out once it's spoiled — wasting money on a tight student budget and contributing to avoidable food waste. There's no simple, low-friction way to know "what do I have, where is it, and what's about to expire" without manually checking every storage spot.

## Who This Is For

**Target users: university students** managing their own food for the first time — typically living alone or in shared housing, cooking for themselves on a limited budget, and juggling groceries across multiple storage spots (fridge, freezer, pantry) without an established system for tracking what's on hand.

## What Foodie Does (v1 / MVP)

- **Inventory management** — add, edit, and remove food items with quantity and unit tracking
- **Storage locations** — organise food across fridge, pantry, freezer, or any custom location
- **Expiry tracking** — see what's expiring soon and what's already expired, at a glance
- **Consumption tracking** — reduce quantities as food is used, with items auto-removed once finished
- **Search & filter** — find items by name, location, or expiry status

**Not in v1** (planned for later versions): barcode/OCR scanning, AI recipe recommendations, household sharing, and waste/spending analytics — see `docs/Foodie_Sprint_Roadmap.md` for the full post-MVP roadmap.

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | React 19 + Vite + TypeScript |
| Backend | ASP.NET Core 10 Web API (C#) |
| Database | PostgreSQL |
| ORM | Entity Framework Core |
| Auth | JWT |
| Testing | Playwright (E2E) |
| Containers | Docker / Docker Compose |
| Deployment | Azure (App Service + Static Web Apps) · Supabase (managed Postgres) |

Full reasoning for every choice above is in `docs/Foodie_Tech_Decisions.md`.

## Repository Structure

```text
Foodie/
│
├── Foodie.sln                   # .NET solution file (Sprint 0)
├── docker-compose.yml           # Local Postgres (+ later API) container config (Sprint 0)
├── .env.example                 # Template for required environment variables (Sprint 0)
├── .gitignore                   # Files/folders excluded from version control (Sprint 0)
├── .gitattributes               # Git line-ending / diff handling rules (Sprint 0)
├── README.md                    # This file (Sprint 0)
├── build.sh                     # Build script, macOS/Linux (Sprint 0)
├── build.cmd                    # Build script, Windows (Sprint 0)
│
├── docs/                        # Planning & decision-record documents
│   ├── Foodie_Architecture_Decisions.md   # 3-tier design, layering pattern, SQL vs NoSQL, diagrams (Sprint 0)
│   ├── Foodie_Tech_Decisions.md           # Stack choices with reasoning, per technology (Sprint 0)
│   ├── Foodie_Tech_Stack.md               # Exact pinned versions of every technology in use (Sprint 0)
│   ├── Foodie_Sprint_Roadmap.md           # Sprint 0–3 plan: tasks, durations, deliverables (Sprint 0)
│   ├── architecture-diagram.puml          # System architecture diagram source (Sprint 0)
│   ├── database-design.puml               # Database/ERD diagram source (Sprint 0)
│   └── PROGRESS_LOG.md                    # Running log of what's been built per sprint (Sprint 0, updated every sprint)
│
├── src/
│   │
│   ├── backend/
│   │   │
│   │   └── Foodie/                # ASP.NET Core Web API (backend)
│   │       ├── Program.cs                       # App entry point, DI registration, middleware pipeline (Sprint 0)
│   │       ├── appsettings.json                 # Base configuration, non-secret (Sprint 0)
│   │       ├── appsettings.Development.json     # Local dev config, gitignored (Sprint 0)
│   │       ├── Dockerfile                       # Container build for the API (Sprint 0)
│   │       │
│   │       ├── Controllers/                     # HTTP in/out only — no business logic
│   │       │   ├── AuthController.cs                # Register, login (Sprint 0)
│   │       │   ├── FoodItemsController.cs           # Inventory CRUD endpoints (Sprint 1)
│   │       │   ├── LocationsController.cs           # Location CRUD + expiry endpoints (Sprint 2)
│   │       │   └── DashboardController.cs           # Consume, search/filter, summary endpoints (Sprint 3)
│   │       │
│   │       ├── Services/                        # Business rules
│   │       │   ├── IAuthService.cs / AuthService.cs                  # JWT issuing/validation, password hashing (Sprint 0)
│   │       │   ├── IFoodItemService.cs / FoodItemService.cs          # Item validation rules (Sprint 1)
│   │       │   ├── ILocationService.cs / LocationService.cs          # Expiry calculation, location-assignment rules (Sprint 2)
│   │       │   └── IDashboardService.cs / DashboardService.cs        # Consumption logic, dashboard aggregation (Sprint 3)
│   │       │
│   │       ├── Repositories/                    # EF Core data access only
│   │       │   ├── IUserRepository.cs / UserRepository.cs                # (Sprint 0)
│   │       │   ├── IFoodItemRepository.cs / FoodItemRepository.cs        # (Sprint 1)
│   │       │   └── ILocationRepository.cs / LocationRepository.cs        # incl. expiry-filtered queries (Sprint 2)
│   │       │
│   │       ├── Models/                          # EF Core entities
│   │       │   ├── User.cs                          # (Sprint 0)
│   │       │   ├── FoodItem.cs                      # (Sprint 1)
│   │       │   └── Location.cs                      # (Sprint 2)
│   │       │
│   │       ├── Data/
│   │       │   ├── FoodieDbContext.cs               # EF Core DbContext (Sprint 0)
│   │       │   └── Migrations/                      # EF Core migrations, one per schema change
│   │       │       ├── InitialCreate                # Users (Sprint 0)
│   │       │       ├── AddFoodItems                 # (Sprint 1)
│   │       │       └── AddLocations                 # (Sprint 2)
│   │       │
│   │       └── Tests/                          # xUnit backend tests
│   │           ├── AuthServiceTests.cs              # (Sprint 0)
│   │           ├── FoodItemServiceTests.cs          # (Sprint 1)
│   │           └── DashboardServiceTests.cs         # Consumption + aggregation logic (Sprint 3)
│   │
│   └── frontend/
│       │
│       └── foodie-web/             # React + Vite frontend
│           ├── package.json                      # (Sprint 0)
│           ├── vite.config.ts                    # (Sprint 0)
│           ├── tsconfig.json                     # (Sprint 0)
│           ├── Dockerfile                        # Container build for the frontend (Sprint 3, deployment)
│           │
│           ├── src/
│           │   ├── main.tsx                          # App entry point (Sprint 0)
│           │   ├── App.tsx                           # Root component, routing (Sprint 0)
│           │   │
│           │   ├── pages/
│           │   │   ├── LoginPage.tsx                # (Sprint 0)
│           │   │   ├── RegisterPage.tsx             # (Sprint 0)
│           │   │   ├── InventoryPage.tsx            # Add/edit/delete item forms (Sprint 1)
│           │   │   ├── LocationsPage.tsx            # Location management UI (Sprint 2)
│           │   │   ├── ExpiryDashboardPage.tsx      # Expiring soon / expired sections (Sprint 2)
│           │   │   └── DashboardPage.tsx            # Summary widgets, search/filter (Sprint 3)
│           │   │
│           │   ├── components/
│           │   │   ├── FoodItemCard.tsx             # (Sprint 1)
│           │   │   ├── FoodItemForm.tsx             # (Sprint 1)
│           │   │   ├── LocationFilter.tsx           # (Sprint 2)
│           │   │   ├── ExpiryBadge.tsx              # (Sprint 2)
│           │   │   ├── EmptyState.tsx               # (Sprint 3)
│           │   │   └── ErrorState.tsx               # (Sprint 3)
│           │   │
│           │   └── api/
│           │       ├── client.ts                    # Base API service layer (Sprint 0)
│           │       ├── authApi.ts                   # (Sprint 0)
│           │       ├── foodItemsApi.ts              # (Sprint 1)
│           │       └── locationsApi.ts              # (Sprint 2)
│           │
│           └── tests/                  # Playwright end-to-end tests (run via foodie-web's package.json)
│               ├── auth.spec.ts                   # Sprint 0
│               ├── inventory.spec.ts              # Sprint 1
│               ├── locations-expiry.spec.ts       # Sprint 2
│               └── consumption-dashboard.spec.ts  # Sprint 3
```

## Getting Started

Setup instructions (local dev environment, environment variables, running the backend/frontend/database) will be added here once Sprint 0 — Project Setup is complete.

## Project Status

Currently in active development as part of a structured 4-week sprint plan (Sprint 0–3) toward a first MVP release. See `docs/Foodie_Sprint_Roadmap.md` for current progress and what's next.
