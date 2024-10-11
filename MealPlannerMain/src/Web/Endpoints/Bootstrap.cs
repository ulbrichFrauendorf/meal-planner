using MealPlanner.Application.Bootstrap.Queries;

namespace MealPlanner.Web.Endpoints;

public class Bootstrap : EndpointGroupBase
{
	public override void Map(WebApplication app)
	{
		app.MapGroup(this)
		   .MapGet(GetAngularApplicationSettings, "angular-settings");
	}

	public async Task<AngularSettingsDto> GetAngularApplicationSettings(ISender sender)
	{
		return await sender.Send(new GetAngularApplicationSettingsQuery());
	}
}
