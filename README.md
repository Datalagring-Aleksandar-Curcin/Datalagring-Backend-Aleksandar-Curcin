# CoursesManager (Data Storage Assignment)

This project is a course management system that manages courses, course sessions, teachers, participants, course status, instance teachers, locations and course registrations using a normalized relational database (EF Core Code First). The solution follows Clean Architecture / DDD and is structured into Presentation, Application, Domain and Infrastructure.

## Tech Stack
- ASP.NET Core Minimal API
- Entity Framework Core (Code First + Migrations)
- SQL Server LocalDB (Windows)

## Prerequisites
- Windows + SQL Server LocalDB installed
- .NET SDK (same version as the project)

## Option C: Run Locally via Terminal

1) Clone and build
git clone <REPO_URL>
cd CoursesManager
dotnet restore
dotnet build

2) Create/Update the database (EF Core Migrations)
dotnet ef database update --project CoursesManager.Infrastructure --startup-project CoursesManager.Presentation

3) Start the API
dotnet run --project CoursesManager.Presentation


## Option B: Run via Visual Studio Package Manager Console (PMC)

1) Open the solution
Open CoursesManager.sln in Visual Studio.

2) Set the correct projects
Set Startup Project: CoursesManager.Presentation
Open Tools → NuGet Package Manager → Package Manager Console
In PMC, set Default project (dropdown) to: CoursesManager.Infrastructure

3) Apply migrations / update database
Type: Update-Database

4) Run
Press F5 (or Ctrl+F5) and open /swagger.


## Notes
- The database is created from EF Core migrations. Local database files (*.mdf/*.ldf) are intentionally not committed to Git.
- The connection string is configured in `CoursesManager.Presentation/appsettings.json` under `ConnectionStrings:LocalDbFile`.
- When the API is running, open the Swagger UI in your browser (the console output shows the exact URL), typically:https://localhost:<port>/swagger



