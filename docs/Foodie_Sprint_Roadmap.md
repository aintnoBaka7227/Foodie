# Foodie — Sprint Roadmap (v1 / MVP)

**Project Goal:** Build a food inventory management application that helps users track food across multiple locations, monitor expiry dates, reduce food waste, and maintain accurate inventory records.

**Tech Stack:** React + Vite · ASP.NET Core Web API · PostgreSQL · EF Core · JWT

---

## Sprint 0 — Planning & Foundation
**Duration:** Week 1 (7 days)
**Goal:** Create project foundations and establish a clear project direction before development begins.

| Task | Est. Duration |
|---|---|
| 1. Define Product Scope and MVP | 1 day |
| 2. User Stories & Requirements | 1 day |
| 3. System Design | 1.5 days |
| 4. Project Setup | 1.5 days |
| 5. Authentication | 2 days |

### Task 1 — Define Product Scope and MVP *(1 day)*
- Product Vision
- Problem Statement
- Target Users
- MVP Scope
- Future Features
- Sprint Roadmap

### Task 2 — User Stories & Requirements *(1 day)*
- Create user stories
- Define functional requirements
- Prioritise MVP features

### Task 3 — System Design *(1.5 days)*
- Use Case Diagram
- Domain Model
- ERD Diagram
- 3-Tier Architecture Diagram

### Task 4 — Project Setup *(1.5 days)*
**Backend:** Create ASP.NET Core Web API project · Configure solution structure · Configure dependency injection
**Frontend:** Create React Vite application · Configure routing · Configure API service layer
**Database:** Configure PostgreSQL · Configure Entity Framework Core · Create initial migration

### Task 5 — Authentication *(2 days)*
**Backend:** User Entity · Register API · Login API · JWT Authentication
**Frontend:** Login Page · Register Page

**Deliverable:** Users can register and login.

---

## Sprint 1 — Inventory Management
**Duration:** Week 2 (7 days)
**Goal:** Allow users to manage food inventory.

| Task | Est. Duration |
|---|---|
| 1. Food Item Entity | 1 day |
| 2. Food Item API | 2 days |
| 3. Inventory UI | 3 days |
| 4. Validation | 1 day |

### Task 1 — Food Item Entity *(1 day)*
Create `FoodItem` model · Configure database relationships
**Fields:** Id, Name, Quantity, Unit, ExpiryDate, LocationId

### Task 2 — Food Item API *(2 days)*
- `GET /api/items`
- `GET /api/items/{id}`
- `POST /api/items`
- `PUT /api/items/{id}`
- `DELETE /api/items/{id}`

### Task 3 — Inventory UI *(3 days)*
- Inventory page
- Add item form
- Edit item form
- Delete item confirmation

### Task 4 — Validation *(1 day)*
- Required fields
- Quantity validation
- Error handling

**Deliverable:** Users can create, view, update and delete food items.

---

## Sprint 2 — Location & Expiry Management
**Duration:** Week 3 (7 days)
**Goal:** Allow users to organise food and track expiry dates.

| Task | Est. Duration |
|---|---|
| 1. Location Entity | 0.5 day |
| 2. Location API | 1 day |
| 3. Assign Food To Locations | 1.5 days |
| 4. Expiry Tracking API | 1 day |
| 5. Expiry Dashboard | 1.5 days |
| 6. Inventory Filtering | 1.5 days |

### Task 1 — Location Entity *(0.5 day)*
**Fields:** Id, Name

### Task 2 — Location API *(1 day)*
- `GET /api/locations`
- `POST /api/locations`
- `PUT /api/locations/{id}`
- `DELETE /api/locations/{id}`

### Task 3 — Assign Food To Locations *(1.5 days)*
- Link FoodItem to Location
- Update forms
- Update database relationships

### Task 4 — Expiry Tracking API *(1 day)*
- `GET /api/items/expired`
- `GET /api/items/expiring-soon`

### Task 5 — Expiry Dashboard *(1.5 days)*
- Expiring Soon section
- Expired Items section

### Task 6 — Inventory Filtering *(1.5 days)*
- Filter by Location
- Sort by Expiry Date

**Deliverable:** Users can organise food across locations and monitor expiry status.

---

## Sprint 3 — Consumption Tracking & MVP Release
**Duration:** Week 4 (7 days)
**Goal:** Complete MVP functionality and prepare for deployment.

| Task | Est. Duration |
|---|---|
| 1. Consumption Tracking | 1 day |
| 2. Dashboard Summary | 1 day |
| 3. Search & Filter | 1 day |
| 4. Error Handling | 1 day |
| 5. Testing | 1 day |
| 6. Documentation | 0.5 day |
| 7. Deployment | 1.5 days |

### Task 1 — Consumption Tracking *(1 day)*
**API:** `PATCH /api/items/{id}/consume`
**Features:** Reduce quantity · Mark consumed · Remove empty items

### Task 2 — Dashboard Summary *(1 day)*
**Widgets:** Total Food Items · Expired Items · Expiring Soon · Total Locations

### Task 3 — Search & Filter *(1 day)*
- Search inventory
- Filter by location
- Filter by expiry status

### Task 4 — Error Handling *(1 day)*
**Backend:** Global exception handling · Validation responses
**Frontend:** Loading states · Empty states · User-friendly errors

### Task 5 — Testing *(1 day)*
**Backend:** API testing
**Frontend:** UI testing

### Task 6 — Documentation *(0.5 day)*
- Update README
- Installation guide
- User guide

### Task 7 — Deployment *(1.5 days)*
- Backend deployment
- Frontend deployment
- Database deployment

**Deliverable:** First working MVP release.

---

## Success Criteria (by end of Sprint 3)

- [ ] User can register and login
- [ ] User can manage food inventory
- [ ] User can manage storage locations
- [ ] User can track expiry dates
- [ ] User can update food quantities
- [ ] User can search inventory
- [ ] Application is deployed
- [ ] MVP is usable by real users
