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

## Solution Context

Minimal Api endpoints are used, but organized in a controller-like fashion. The magic is being done with reflection in the WebApplicationExtensions class inside the web project. Every endpoint needs to inherit the EndpointGroupBase class, in order for the Endpoints to be utilized correctly.

The Command AddRecipeIngredientCommand showcases the full power of this Application, by utilizing validition behaviours included in Mediatr Pattern as well as the event handler, that is set up in the domain layer, but dispatched by way of Data base interceptor. "DispatchDomainEventsInterceptor". Every data base entity is audited using the AuditableEntityInterceptor Class. Thus elimination the need for seting up temporal Tables in the SQL database.

AuthorizeAttribute has been implemented in the application layer, in order to granualary control the authorization at a business rule level.

I have added only CRUD implementation of the Recipes Domain Entity. And the Functionality as requested.

Nswag is used to generate the Enpoint http Clients as well as the data models reqired by the Endpoints.(This truly creates a balance between speed of development and structure, whis is easy to maintain sing the backend and basically copied to the front-end.)

No Repository pattern used in source. Opted for a DbContext interface in the Application Layer, Which still adheres strictly to CLEAN architecture.

_(I am sure discussions will be prompted...)_

## DB migrations

(If you are interested how to do it, with the architecture I used)

Add

```
dotnet ef migrations add "SampleMigration" --project src\Infrastructure --startup-project src\Web --output-dir Data\Migrations
```

Remove

```
dotnet ef migrations remove --project src\Infrastructure --startup-project src\Web
```

## Tests

Docker is needed to run the Application.FunctionalTests.

Mapper tests will search for any Mappers used and test nmapped properties.
