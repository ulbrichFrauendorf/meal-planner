# Meal Planner Web Application (Job Interview Task)

## Run

To run the web application:
I have created a batch file to run both the meal planner solution as well as the auth solution.

(Full disclaimer, I have forked one of my personal projects to use as the auth solution. It is still very much incomplete)

Run from the MealPlanner folder

```bash
.\run.bat
```

In your browser, Navigate to https://localhost:44447/

## Login Users:

User: user@mealplanner.web.za - P@ssw0rd1

Administrator: admin@mealplanner.web.za - P@ssw0rd1

## Solution Overview

The project utilizes **Minimal API** endpoints organized similarly to traditional controllers. The reflection logic in the `WebApplicationExtensions` class (located in the `Web` project) dynamically manages these endpoints.

Each API endpoint must inherit from the `EndpointGroupBase` class to ensure proper functionality.

### Key Features

- **Command Pattern**:  
  The `AddRecipeIngredientCommand` highlights the core capabilities of the application by utilizing:

  - Validation behaviors via **MediatR**.
  - Event handling configured in the **domain layer** and dispatched using the `DispatchDomainEventsInterceptor`.

- **Auditing**:  
  Every database entity is audited using the `AuditableEntityInterceptor`, which eliminates the need for SQL temporal tables.

- **Authorization**:  
  The `AuthorizeAttribute` is implemented in the **application layer** to provide fine-grained control over authorization based on business rules.

- **Domain Implementation**:  
  The application currently implements basic CRUD operations for the `Recipes` domain entity along with the requested functionality.

- **NSwag**:  
  NSwag is utilized to generate:

  - HTTP clients for the endpoints.
  - Data models required by the API, providing a balance between rapid development and maintainable code that is consistent across both the backend and frontend.

- **Clean Architecture**:  
  The solution adheres strictly to Clean Architecture principles. Instead of using a repository pattern, a `DbContext` interface is implemented in the **application layer**, maintaining architectural integrity.

> **Note**: This README is designed to be concise to encourage further discussion during the interview.

## Database Migrations

For those interested in handling migrations within this architecture, here are the commands:

### Adding a Migration:

(If you are interested how to do it, with the architecture I used)

Add

```

dotnet ef migrations add "SampleMigration" --project src\Infrastructure --startup-project src\Web --output-dir Data\Migrations

```

Remove

```

dotnet ef migrations remove --project src\Infrastructure --startup-project src\Web

```

## Testing

- **Docker**:  
  The `Application.FunctionalTests` requires Docker to be running.

- **Mapper Tests**:  
  The tests will automatically discover all **AutoMapper** configurations used in the project and ensure that properties are correctly mapped between source and target models.

## Final Notes

- I used my GitHub logo across all sites; I felt that designing a new logo was unnecessary given the project's focus.

---
