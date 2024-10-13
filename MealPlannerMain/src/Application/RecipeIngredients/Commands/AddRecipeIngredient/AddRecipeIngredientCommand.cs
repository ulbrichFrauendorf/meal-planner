using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Application.Common.Security;
using MealPlanner.Domain.Constants;
using MealPlanner.Domain.Entities;
using MealPlanner.Domain.Events;

namespace MealPlanner.Application.RecipeIngredients.Commands.AddRecipeIngredient;

[Authorize(Policy = Policies.Admin)]
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

		foreach (var entity in entities)
		{
			entity.AddDomainEvent(new IngredientAddedEvent(entity));
		}

		await context.SaveChangesAsync(cancellationToken);

		return entities.Select(e => e.Id).ToList();
	}
}
