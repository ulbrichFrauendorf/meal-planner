using MealPlanner.Application.Common.Interfaces;

namespace MealPlanner.Web.Services;

public class CurrentUser(IIdentityService identityService) : IUser
{
	private string? _id;

	public string? Id => identityService.GetUserId() ?? _id;

	public Guid? Guid => string.IsNullOrWhiteSpace(Id) ? null : new Guid(Id!);

	public string? Email => identityService.GetUserEmail();

	public void SetUser(string id)
	{
		_id = id;
	}
}
