using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Domain.Entities;

namespace MealPlanner.Application.RecipeIngredients.Commands.AddRecipeIngredient;

public record AddRecipeIngredientCommand() : IRequest<List<Guid>>
{
	public Guid RecipeId { get; init; }
	public List<Guid> IngredientIds { get; init; } = [];
};

public class AddRecipeIngredientCommandHandler(IApplicationDbContext context) : IRequestHandler<AddRecipeIngredientCommand, List<Guid>>
{
	public async Task<List<Guid>> Handle(AddRecipeIngredientCommand request, CancellationToken cancellationToken)
	{
		var entities = request.IngredientIds.Select(ingredientId => new RecipeIngredient
		{
			RecipeId = request.RecipeId,
			IngredientId = ingredientId
		});

		context.RecipeIngredients.AddRange(entities);

		await context.SaveChangesAsync(cancellationToken);

		return entities.Select(e => e.Id).ToList();
	}
}
