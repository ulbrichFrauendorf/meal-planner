# Meal Planner Web Application (Job Interview Task)

## Build

Run `dotnet build -tl` to build the solution.

## Run

To run the web application:
```bash
cd .\src\Web\
dotnet watch run
```

## DB migrations
Add 
```
dotnet ef migrations add "SampleMigration" --project src\Infrastructure --startup-project src\Web --output-dir Data\Migrations
```

Remove 
```
dotnet ef migrations remove --project src\Infrastructure --startup-project src\Web
```