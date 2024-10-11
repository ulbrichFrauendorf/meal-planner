using System.Data.Common;

namespace MealPlanner.Application.FunctionalTests;

public interface ITestDatabase
{
	Task InitialiseAsync();

	DbConnection GetConnection();

	Task ResetAsync();

	Task DisposeAsync();
}
