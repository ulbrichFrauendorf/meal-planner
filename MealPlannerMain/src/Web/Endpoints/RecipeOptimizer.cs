using MealPlanner.Application.RecipeOptimizer;
using MealPlanner.Application.RecipeOptimizer.Queries;

namespace MealPlanner.Web.Endpoints;

public class RecipeOptimizer : EndpointGroupBase
{
	public override void Map(WebApplication app)
	{
		app.MapGroup(this)
			.RequireAuthorization()
			.MapPost(GetOptimalRecipeInformation);
	}

	public async Task<CalculationResult> GetOptimalRecipeInformation(ISender sender, GetOptimalRecipeInformationQuery query)
	{
		return await sender.Send(query);
	}
}
