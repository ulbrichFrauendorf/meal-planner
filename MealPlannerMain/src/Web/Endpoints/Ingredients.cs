using MealPlanner.Application.Tenants;
using MealPlanner.Application.Tenants.Queries;

namespace MealPlanner.Web.Endpoints;

public class Ingredients : EndpointGroupBase
{
	public override void Map(WebApplication app)
	{
		app.MapGroup(this).RequireAuthorization()
			.MapGet(GetAllIngredients, "all")
			.MapGet(GetAvailableIngredients,"available");
	}

	public Task<IReadOnlyCollection<IngredientDto>> GetAllIngredients(
		ISender sender,
		[AsParameters] GetAllIngredientsQuery query
	)
	{
		return sender.Send(query);
	}

	public Task<IReadOnlyCollection<IngredientDto>> GetAvailableIngredients(
		ISender sender,
		[AsParameters] GetAvailableIngredientsQuery query
	)
	{
		return sender.Send(query);
	}
}
