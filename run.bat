@echo off
REM Get the directory of the batch file
set script_dir=%~dp0

REM Define relative paths to the solutions' directories from the batch file location
set solution_auth_path=%script_dir%\MealPlannerAuthentication
set solution_webapp_path=%script_dir%\MealPlannerMain

REM Echo variables for debugging
echo Script directory: %script_dir%
echo First solution path: %solution_auth_path%
echo Second solution path: %solution_webapp_path%

REM Change directory to the auth solution directory
echo Building and running auth project...
cd /d %solution_auth_path%
dotnet build -tl
start dotnet dotnet watch run --project "src\Web\Web.csproj"

REM Change directory to the webapp solution's directory
echo Building and running webapp project...
cd /d %solution_webapp_path%
dotnet build -tl
start dotnet watch run --project "src\Web\Web.csproj"

REM Optional: Keep the terminal open
pause
