# CoursesManager (Data Storage Assignment)

This project is a course management system that manages courses, course sessions, teachers, participants, and registrations using a normalized relational database (EF Core Code First). The solution follows Clean Architecture / DDD and is structured into Presentation, Application, Domain, Infrastructure, and Tests.

## Tech Stack
- ASP.NET Core Minimal API
- Entity Framework Core (Code First + Migrations)
- SQL Server LocalDB (Windows)
- Automated tests (unit/integration)

## Prerequisites
- Windows + SQL Server LocalDB installed
- .NET SDK (same version as the project)

## Run Locally (PowerShell)

1) Clone and build
git clone <REPO_URL>
cd CoursesManager
dotnet restore
dotnet build

2) Create/Update the database (EF Core Migrations)
dotnet ef database update --project CoursesManager.Infrastructure --startup-project CoursesManager.Presentation

3) Start the API
dotnet run --project CoursesManager.Presentation

## Notes
- The database is created from EF Core migrations. Local database files (*.mdf/*.ldf) are intentionally not committed to Git.
- The connection string is configured in `CoursesManager.Presentation/appsettings.json` under `ConnectionStrings:DefaultConnection`.
- If Swagger/OpenAPI is enabled, open `/swagger` after starting the API.
