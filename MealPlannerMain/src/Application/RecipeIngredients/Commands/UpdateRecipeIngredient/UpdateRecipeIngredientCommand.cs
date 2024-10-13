using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Application.Common.Security;
using MealPlanner.Domain.Constants;

namespace MealPlanner.Application.RecipeIngredients.Commands.UpdateRecipeIngredient;

[Authorize(Policy = Policies.Admin)]
public record UpdateRecipeIngredientCommand() : IRequest
{
	public Guid RecipeIngredientId { get; init; }
	public int Quantity { get; init; }
};

public class UpdateRecipeIngredientCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateRecipeIngredientCommand>
{
	public async Task Handle(UpdateRecipeIngredientCommand request, CancellationToken cancellationToken)
	{
		var entity = await context.RecipeIngredients
			.FindAsync(new object[] { request.RecipeIngredientId }, cancellationToken);

		Guard.Against.NotFound(request.RecipeIngredientId, entity);

		entity.Quantity = request.Quantity;

		await context.SaveChangesAsync(cancellationToken);
	}
}
