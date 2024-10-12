using MealPlanner.Application.Recipes;
using MealPlanner.Application.Recipes.Commands.AddRecipeCommand;
using MealPlanner.Application.Recipes.Queries.GetAllRecipes;

namespace MealPlanner.Web.Endpoints;

public class Recipes : EndpointGroupBase
{
	public override void Map(WebApplication app)
	{
		app.MapGroup(this).RequireAuthorization()
			.MapGet(GetAllRecipes)
			.MapPost(AddRecipe);
	}

	public async Task<IReadOnlyCollection<RecipeDto>> GetAllRecipes(
		ISender sender,
		[AsParameters] GetAllRecipesQuery query
	)
	{
		return await sender.Send(query);
	}

	public Task<Guid> AddRecipe(ISender sender, AddRecipeCommand command)
	{
		return sender.Send(command);
	}
}
