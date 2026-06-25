# Foodie — Database Design Decisions

---

## 1. Tables (Entities)

Three tables for the MVP: **Users**, **Locations**, **FoodItems**.

**Chosen over:** A single denormalized table; a separate `Consumption` table.

Each table represents one real-world concept with no repeating groups (1NF) and no column duplicated across tables (3NF check passed — see Section 5). `Consumption` was deliberately *not* split into its own table: Sprint 3's consumption tracking is a quantity update and removal on the existing `FoodItems` row, not a new historical record. Adding a `Consumption` table now would be premature — there's no requirement yet for a consumption *history* (e.g. "show me everything I ate this month"), only for current inventory state. Revisit if that requirement appears post-MVP.

---

## 2. Primary Keys — Surrogate, not Natural

**Chosen over:** Natural keys (e.g. `Email` as PK on Users, `Name` as PK on Locations).

All three tables use an auto-incrementing `Id` (INT) as a surrogate primary key.

Surrogate keys were chosen because: none of Foodie's entities have a value that's both naturally unique *and* guaranteed never to change. `Email` could be a candidate key on `Users` (and is enforced as `UNIQUE`, see Section 4), but using it as the PK would mean every FK referencing a user breaks if the email is ever updated. `Location.Name` ("Fridge", "Pantry") isn't even unique within a household, let alone across all users. Surrogate keys avoid both problems and keep every FK column a stable, opaque integer.

---

## 3. Foreign Keys & Relationship Cardinality

All three relationships in Foodie are **one-to-many**; there are no many-to-many relationships in the MVP, so no junction table is needed.

| Relationship | Cardinality | FK column |
|---|---|---|
| Users → Locations | 1 : N | `Locations.UserId` |
| Users → FoodItems | 1 : N | `FoodItems.UserId` |
| Locations → FoodItems | 1 : N | `FoodItems.LocationId` |

**Why no junction table:** the article's many-to-many pattern (e.g. students↔courses) applies when both sides can have multiple of the other. That's not true here — one location belongs to exactly one user, one food item belongs to exactly one location. If a future feature allowed *shared households* (multiple users managing the same location), that would become many-to-many and require a junction table (e.g. `LocationMembers`). Out of scope for MVP.

---

## 4. NOT NULL / Modality Decisions

This is the one decision in the schema that needed real back-and-forth, because the two natural-sounding rules conflicted with each other:

- **Rule A (business requirement):** every `FoodItem` must have a location — you shouldn't be able to add food with nowhere to put it.
- **Rule B (deletion behavior):** deleting a `Location` should not destroy the `FoodItems` stored there; it should just unassign them so the user can move them elsewhere.

These can't both be enforced as hard database constraints simultaneously: `ON DELETE SET NULL` requires the FK column to be **nullable**, but "every item must have a location" requires it to be **NOT NULL**. A column can't be both.

**Decision:** `FoodItems.LocationId` is **nullable at the database level**, with `ON DELETE SET NULL`. The "every item must have a location" rule is enforced at the **application layer** instead — the create-item form requires a location to be selected before submission, but the database itself permits a NULL (which only ever occurs as a side effect of a location being deleted, not through normal item creation).

**Why this split, not the other way around:** Enforcing "required" at the DB level (`NOT NULL`) would force `ON DELETE` to be either `CASCADE` (deletes the user's food data — too destructive for what should be a low-stakes action like renaming/removing a shelf) or `NO ACTION`/`RESTRICT` (blocks the delete entirely until every item is reassigned — overly strict for a casual organizing app). `SET NULL` is the better UX: delete the location, items survive as "unassigned," user reassigns them later through the normal edit flow. The trade-off is that referential integrity alone can no longer guarantee every item has a location — that responsibility now sits with the application/service layer (`FoodItemService`), which is an explicit, documented gap rather than an accidental one.

All other FK columns (`Locations.UserId`, `FoodItems.UserId`) are **NOT NULL** — a location or food item with no owning user is meaningless, and there's no equivalent "soft unassign" use case for the user relationship; if a user is deleted, their data should go with them (`ON DELETE CASCADE`).

---

## 5. Normalization Check (1NF / 2NF / 3NF)

Walked through deliberately rather than assumed:

- **1NF (atomic values):** Every column holds a single value — `Quantity`, `Unit`, and `ExpiryDate` are separate columns rather than one combined/delimited field. No repeating groups.
- **2NF (no partial dependency):** N/A in the strict sense — no table uses a composite primary key, so partial dependency (which only applies to composite keys) can't occur.
- **3NF (no transitive dependency):** Checked `FoodItems` specifically, since it's the table most at risk (it has two FKs). Every non-key column (`Name`, `Quantity`, `Unit`, `ExpiryDate`) depends only on `FoodItems.Id` — none of them are derived from or duplicated via `LocationId` or `UserId`. For example, `Locations.Name` is **not** copied into `FoodItems` (which would let it go stale if the location were renamed) — callers join to `Locations` instead. This is the same trap the reference article's `class_name` example demonstrates.

---

## 6. Data Integrity Rules

Following the article's three integrity types:

| Type | Applied as |
|---|---|
| **Entity integrity** | Every table has a non-null, unique surrogate PK (`Id`) |
| **Referential integrity** | All FKs declared with explicit `ON DELETE` behavior (Section 4) — none left as an undefined default |
| **Domain integrity** | `Quantity` constrained to `>= 0` (`CHECK` constraint); `Email` constrained `UNIQUE` |

---

## 7. What's Deliberately Out of Scope (MVP)

- **Consumption history table** — not needed until a feature requires historical reporting, not just current state (Section 1)
- **Shared households / multi-user locations** — would require a many-to-many junction table; not in Sprint 0-3 scope (Section 3)
- **Soft-delete on Users/Locations/FoodItems** — MVP uses hard deletes with cascade/set-null; soft-delete (an `IsDeleted` flag) would be a later addition if an "undo delete" or audit-trail feature is requested
- **Physical address on Locations** — MVP assumes one home per user, so `Locations` has no `Address` field. Adding one now would duplicate the same address on every row for that user (a 3NF violation — address depends on the *home*, not the *location*). When multi-home support is added, the fix is a new `Homes` table (`Id`, `Address`, owning `User`) with `Locations` gaining a `HomeId` FK — not an `Address` column on `Locations` itself.
