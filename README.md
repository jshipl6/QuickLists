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

## Week 11 – Separation of Concerns / Dependency Injection

This week I moved the QuickLists business logic out of the controller and into a dedicated service to improve separation of concerns. I created an interface `ITaskService` that defines the operations my app needs: load all tasks, load a single task, add a task, toggle completion, and delete. I then implemented that interface in `TaskService`, which uses the EF Core `QuickListsContext` to talk to the database.

I registered the service with the DI container using `AddScoped<ITaskService, TaskService>()` in `Program.cs`. Scoped lifetime matches EF Core usage because each web request gets its own DbContext instance. The `TasksController` now depends on `ITaskService` instead of the DbContext. This keeps the controller thin and focused on HTTP concerns such as model binding, validation, and returning views, while the service contains all of the data access and domain logic.

This refactor makes the code easier to test and change. For example, I can swap in a fake implementation of `ITaskService` for unit tests without touching the controller. It also sets me up for future features like logging inside the service or adding caching around reads. Screenshots in this folder show the service files, DI registration in `Program.cs`, and the controller calling the service.

## Week 12 - CRUD  

This week I implemented the full CRUD feature for the QuickLists application. The goal was to deliver one complete end to end vertical slice that shows async data access, validation, and at least one working write action. I focused on the Tasks section of the app and expanded it so users can create tasks, view tasks, and read details for each task.

The first step was adding full async data access. I updated the service methods to use ToListAsync, FirstOrDefaultAsync, AddAsync, SaveChangesAsync, and similar operations. This ensured that the controller could await results correctly and that the entire data flow was non blocking. The QuickListsContext class already had the DbSet for TaskItem so all that was required was to clean up the interfaces and make sure each method returned the right type. After debugging a few issues and fixing method signatures in the interface, the service compiled and functioned as expected.

Next I added validation feedback for the create page. The TaskItem model already had a required attribute on the Title property, so the controller only needed to return the same view when the model state was invalid. This allowed the user to see validation messages without losing the data they typed. The view was updated to show validation messages using the Tag Helpers built into ASP.NET Core. Once that was done the create form enforced the required rule correctly.

I also implemented a task details page. This included a new action in the controller and a new Razor view. The details page shows the task title and completion state. This completed the requirement for at least one read operation beyond the index list.

## Week 13 - Diagnostics Feature

For this week I added a diagnostics feature to my QuickLists MVC application. The goal was to expose a health endpoint that returns a clear status along with at least one real dependency check. I decided to build a controller named DiagnosticsController and add a route at slash health. This route returns a JSON object that shows whether the application is running correctly along with the state of the database connection.

To make sure the endpoint included a real dependency check I used the Entity Framework Core database context and executed a simple SQL command. This confirms that the database is reachable without exposing anything sensitive. If the query fails the endpoint reports that the database connection is not working. If it succeeds the endpoint reports that everything is healthy. This gives a straightforward snapshot of system health that can be useful in both development and production environments.

The biggest challenge was deciding how much information to include in the health response. I wanted the output to be clear and helpful but I also wanted to avoid leaking anything that should not be public. Returning a simple status flag for the database plus a timestamp kept it clean and safe.

The most useful part of this assignment was understanding how simple diagnostic endpoints can help monitor an application. In real world situations this type of health route can be used by load balancers, container orchestrators, uptime monitors and automated deployment systems to verify that an application is alive and able to reach its required dependencies. This type of feature makes the application more reliable and easier to maintain.

Finally I reviewed and tested the entire flow. Creating a task works as expected. The list updates after creation. The details page loads and shows the correct item. All async calls work. Validation works. The project now includes every required part of the assignment.


