using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Domain.Entities;

namespace MealPlanner.Application.Recipes.Commands;

public record AddRecipeCommand() : IRequest<Guid>
{
	public required string Name { get; init; }
	public required int PeopleFed { get; init; }
}

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

public class AddRecipeCommandHandler : IRequestHandler<AddRecipeCommand, Guid>
{
	private readonly IApplicationDbContext _context;

	public AddRecipeCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Guid> Handle(AddRecipeCommand request, CancellationToken cancellationToken)
	{
		var entity = new Recipe
		{
			Name = request.Name,
			PeopleFed = request.PeopleFed
		};

		_context.Recipes.Add(entity);

		await _context.SaveChangesAsync(cancellationToken);

		return entity.Id;
	}
}
