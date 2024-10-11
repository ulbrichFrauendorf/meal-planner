using MealPlanner.Application.Common.Interfaces;

namespace MealPlanner.Application.Recipes.Commands;

public record RemoveRecipeIngredientCommand(Guid Id) : IRequest;

public class RemoveRecipeIngredientCommandHandler : IRequestHandler<RemoveRecipeIngredientCommand>
{
	private readonly IApplicationDbContext _context;

	public RemoveRecipeIngredientCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task Handle(RemoveRecipeIngredientCommand request, CancellationToken cancellationToken)
	{
		var entity = await _context.RecipeIngredients
			.Where(r => r.Id == request.Id)
			.SingleOrDefaultAsync(cancellationToken);

		Guard.Against.NotFound(request.Id, entity);

		_context.RecipeIngredients.Remove(entity);

		await _context.SaveChangesAsync(cancellationToken);
	}
}
