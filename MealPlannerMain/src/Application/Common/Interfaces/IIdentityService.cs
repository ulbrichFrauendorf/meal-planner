namespace MealPlanner.Application.Common.Interfaces;

public interface IIdentityService
{
	string? GetUserId();
	string? GetUserEmail();
	string? GetUserName();
	bool IsInRole(string role);
	List<string> GetUserClaims();
	Task<bool> AuthorizeAsync(string policy);
	bool HasSystemClaim(string claim);
}
