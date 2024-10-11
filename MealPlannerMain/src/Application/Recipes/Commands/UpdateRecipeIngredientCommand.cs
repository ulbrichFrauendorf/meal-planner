using MealPlanner.Application.Common.Interfaces;

namespace MealPlanner.Application.Recipes.Commands;

public record UpdateRecipeIngredientCommand() : IRequest
{
	public Guid RecipeIngredientId { get; init; }
	public int Quantity { get; init; }
};

public class UpdateRecipeIngredientCommandHandler : IRequestHandler<UpdateRecipeIngredientCommand>
{
	private readonly IApplicationDbContext _context;

	public UpdateRecipeIngredientCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task Handle(UpdateRecipeIngredientCommand request, CancellationToken cancellationToken)
	{
		var entity = await _context.RecipeIngredients
			.FindAsync(new object[] { request.RecipeIngredientId }, cancellationToken);

		Guard.Against.NotFound(request.RecipeIngredientId, entity);

		entity.Quantity = request.Quantity;

		await _context.SaveChangesAsync(cancellationToken);
	}
}
