using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Domain.Entities;

namespace MealPlanner.Application.Recipes.Commands.AddRecipeCommand;

public record AddRecipeCommand() : IRequest<Guid>
{
	public required string Name { get; init; }
	public required int PeopleFed { get; init; }
}

public class AddRecipeCommandHandler(IApplicationDbContext context) : IRequestHandler<AddRecipeCommand, Guid>
{
	public async Task<Guid> Handle(AddRecipeCommand request, CancellationToken cancellationToken)
	{
		var entity = new Recipe
		{
			Name = request.Name,
			PeopleFed = request.PeopleFed
		};

		context.Recipes.Add(entity);

		await context.SaveChangesAsync(cancellationToken);

		return entity.Id;
	}
}
