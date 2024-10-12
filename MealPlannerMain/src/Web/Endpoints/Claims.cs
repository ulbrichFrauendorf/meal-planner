using MealPlanner.Application.Claims.Queries.HasClaim;

namespace MealPlanner.Web.Endpoints;

public class Claims() : EndpointGroupBase
{
	public override void Map(WebApplication app)
	{
		app.MapGroup(this).RequireAuthorization().MapGet(HasClaim);
	}

	public async Task<bool> HasClaim(ISender sender, string claimName)
	{
		return await sender.Send(new HasClaimQuery { ClaimName = claimName });
	}
}
