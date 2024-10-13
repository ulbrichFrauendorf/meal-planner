@echo off

set script_dir=%~dp0

set solution_auth_path=%script_dir%\MealPlannerAuthentication
set solution_webapp_path=%script_dir%\MealPlannerMain

echo Building and running auth project...
cd /d %solution_auth_path%
dotnet build -tl
start dotnet dotnet watch run --project "src\Web\Web.csproj"

echo Building and running webapp project...
cd /d %solution_webapp_path%
dotnet build -tl
start dotnet watch run --project "src\Web\Web.csproj"

Start-Process "https://localhost:44447"
