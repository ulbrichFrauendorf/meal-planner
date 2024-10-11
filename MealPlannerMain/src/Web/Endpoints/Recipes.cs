using MealPlanner.Application.Common.Models;
using MealPlanner.Application.Recipes;
using MealPlanner.Application.Recipes.Commands;
using MealPlanner.Application.Recipes.Queries;

namespace MealPlanner.Web.Endpoints;

public class Recipes : EndpointGroupBase
{
	public override void Map(WebApplication app)
	{
		app.MapGroup(this).RequireAuthorization()
			.MapGet(GetAllRecipes, "all")
			.MapPost(AddRecipe)
			.MapGet(GetRecipeIngredients,"ingredients")
			.MapPost(AddRecipeIngredients, "ingredient")
			.MapPut(UpdateRecipeIngredient, "ingredient/{id}")
			.MapDelete(RemoveRecipeIngredient, "ingredient/{id}");
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

	public async  Task<IResult> RemoveRecipeIngredient(
		ISender sender, Guid id	)
	{
		await sender.Send(new RemoveRecipeIngredientCommand(id));
		return Results.NoContent();
	}
}
