using MealPlanner.Application.Common.Interfaces;

namespace MealPlanner.Application.Recipes.Commands.AddRecipeCommand;

public class AddRecipeCommandValidator : AbstractValidator<AddRecipeCommand>
{
	private readonly IApplicationDbContext _context;

	public AddRecipeCommandValidator(IApplicationDbContext context)
	{
		_context = context;

		RuleFor(v => v.Name)
			.NotEmpty()
			.MaximumLength(200)
			.MustAsync(BeUniqueTitle)
				.WithMessage("'{PropertyName}' must be unique.")
				.WithErrorCode("Unique");
	}

	public async Task<bool> BeUniqueTitle(string name, CancellationToken cancellationToken)
	{
		return await _context.Recipes
			.AllAsync(l => l.Name != name, cancellationToken);
	}
}
