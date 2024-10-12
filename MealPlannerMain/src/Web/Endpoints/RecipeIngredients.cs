using MealPlanner.Application.RecipeIngredients;
using MealPlanner.Application.RecipeIngredients.Commands.AddRecipeIngredient;
using MealPlanner.Application.RecipeIngredients.Commands.RemoveRecipeIngredient;
using MealPlanner.Application.RecipeIngredients.Commands.UpdateRecipeIngredient;
using MealPlanner.Application.RecipeIngredients.Queries.GetRecipeIngredients;

namespace MealPlanner.Web.Endpoints;

public class RecipeIngredients : EndpointGroupBase
{
	public override void Map(WebApplication app)
	{
		app.MapGroup(this).RequireAuthorization()
			.MapGet(GetRecipeIngredients)
			.MapPost(AddRecipeIngredients)
			.MapPut(UpdateRecipeIngredient, "{id}")
			.MapDelete(RemoveRecipeIngredient, "{id}");
	}

	public async Task<IReadOnlyCollection<RecipeIngredientDto>> GetRecipeIngredients(
		ISender sender,
		[AsParameters] GetRecipeIngredientsQuery query
	)
	{
		return await sender.Send(query);
	}

	public Task<List<Guid>> AddRecipeIngredients(ISender sender, AddRecipeIngredientCommand command)
	{
		return sender.Send(command);
	}

	public async Task<IResult> UpdateRecipeIngredient(ISender sender, Guid id, UpdateRecipeIngredientCommand command)
	{
		if (id != command.RecipeIngredientId) return Results.BadRequest();
		await sender.Send(command);
		return Results.NoContent();
	}

	public async Task<IResult> RemoveRecipeIngredient(
		ISender sender, Guid id)
	{
		await sender.Send(new RemoveRecipeIngredientCommand(id));
		return Results.NoContent();
	}
}
