# QuickLists

A tiny ASP.NET Core MVC app to track lists and their items. Kept small on purpose so each weekly concept is obvious and fast to grade. Dev DB is SQL Server LocalDB. Deployment target is Azure App Service. Health checks, DI, structured logging, and a small stored procedure are planned.

## Planning Table

| Week | Concept | Feature | Goal | Acceptance Criteria | Evidence in README.md | Test Plan |
|---|---|---|---|---|---|---|
| 10 | Modeling | Create `List` and `Item` entities with one-to-many | App can store lists and their items | Tables created. FK works. Migration applied. Seed shows one list with two items | Code link. Short write-up. Screenshot of migration and rows | Run migration. Confirm tables and FK. Home shows seeded list |
| 11 | Separation of Concerns and DI | Add `IListService` to wrap item ops | Move CRUD logic out of controllers | Service registered in DI. Controllers use ctor injection. Unit test stubs compile | Code link. One paragraph on why service | Hit endpoints and confirm service is called |
| 12 | CRUD | Create and edit forms for items | Users can add and edit items | Valid post saves. Edit updates. Validation messages show when invalid | Screenshots of create and edit. Code links | Add item then edit. Confirm DB changes. Invalid post shows validation |
| 13 | Diagnostics | Add `/healthz` with DB check | Report healthy when DB reachable | `/healthz` returns 200 when DB up. 503 when connection is wrong | curl output screenshots for both states | Toggle connection and curl endpoint |
| 14 | Logging | Structured logs for item lifecycle | Capture traces | Log created with `EventId=ItemCreated` and properties `ListId`, `ItemId`, `Title` | Log snippet and code | Create and edit an item then inspect logs |
| 15 | Stored Procedures | `dbo.GetTopListsByOpenItems` plus page | Leaderboard of lists with most open items | SP created via migration. Page uses `FromSqlInterpolated` to show top five | SP script in README. Screenshot of page | Add items, mark some complete, confirm ordering |
| 16 | Deployment | Azure App Service with secrets | Public URL runs prod settings | App builds and runs on Azure. Connection string in App Service. `/healthz` reachable | Deployed URL and screenshots | Visit URL, open home and `/healthz` |

## Week 10 – Modeling

This week I implemented the data model for the QuickLists app. I added a `TaskItem` entity with fields for Id, Title, and IsComplete, and created a `QuickListsContext` that inherits from `DbContext` and includes a `DbSet<TaskItem>`.

I connected the app to an SQLite database using `appsettings.json` and registered the DbContext inside `Program.cs`. After that, I generated the initial migration and updated the database.

### What was completed:
- Created TaskItem entity (Id, Title, IsComplete)
- Added QuickListsContext with DbSet<TaskItem>
- Registered DbContext in Program.cs with SQLite connection
- Ran `Add-Migration InitialCreate` and `Update-Database`
- Verified that SQLite database file was created

This completes the Week 10 requirement of defining the data model and setting up the database layer for future CRUD functionality.

