namespace MealPlanner.Application.Common.Interfaces;

public interface IUser
{
	string? Id { get; }
	Guid? Guid { get; }
	string? Email { get; }
	void SetUser(string id);
}
