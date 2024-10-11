using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Domain.Entities;

namespace MealPlanner.Application.Recipes.Commands;

public record AddRecipeIngredientCommand() : IRequest<List<Guid>>
{
	public Guid RecipeId { get; init; }
	public List<Guid> IngredientIds { get; init; } = [];
};

public class AddRecipeIngredientValidator : AbstractValidator<AddRecipeIngredientCommand>
{
	private readonly IApplicationDbContext _context;

	public AddRecipeIngredientValidator(IApplicationDbContext context)
	{
		_context = context;

		RuleFor(v => v.RecipeId)
			.MustAsync(RecipeExists)
				.WithMessage("'{PropertyName}' must exist.")
				.WithErrorCode("NotFound");
	}

	public async Task<bool> RecipeExists(Guid recipeId, CancellationToken cancellationToken)
	{
		return await _context.Recipes
			.AnyAsync(l => l.Id != recipeId, cancellationToken);
	}
}

public class AddRecipeIngredientCommandHandler : IRequestHandler<AddRecipeIngredientCommand, List<Guid>>
{
	private readonly IApplicationDbContext _context;

	public AddRecipeIngredientCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<Guid>> Handle(AddRecipeIngredientCommand request, CancellationToken cancellationToken)
	{
		var entities = request.IngredientIds.Select(ingredientId => new RecipeIngredient
		{
			RecipeId = request.RecipeId,
			IngredientId = ingredientId
		});

		_context.RecipeIngredients.AddRange(entities);

		await _context.SaveChangesAsync(cancellationToken);

		return entities.Select(e=>e.Id).ToList();
	}
}
