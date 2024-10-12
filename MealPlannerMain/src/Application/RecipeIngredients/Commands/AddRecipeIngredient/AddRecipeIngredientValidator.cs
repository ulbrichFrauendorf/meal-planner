using MealPlanner.Application.Common.Interfaces;

namespace MealPlanner.Application.RecipeIngredients.Commands.AddRecipeIngredient;

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
