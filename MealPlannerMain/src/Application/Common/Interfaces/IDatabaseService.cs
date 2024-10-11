namespace MealPlanner.Application.Common.Interfaces;

public interface IApplicationDatabaseService
{
	Task ExecuteSqlCommandAsync(string sql, params object[] parameters);
}
