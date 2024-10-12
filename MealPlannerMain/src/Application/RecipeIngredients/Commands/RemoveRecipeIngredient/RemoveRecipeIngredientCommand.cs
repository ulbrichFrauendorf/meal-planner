﻿using MealPlanner.Application.Common.Interfaces;

namespace MealPlanner.Application.RecipeIngredients.Commands.RemoveRecipeIngredient;

public record RemoveRecipeIngredientCommand(Guid Id) : IRequest;

public class RemoveRecipeIngredientCommandHandler(IApplicationDbContext context) : IRequestHandler<RemoveRecipeIngredientCommand>
{
	public async Task Handle(RemoveRecipeIngredientCommand request, CancellationToken cancellationToken)
	{
		var entity = await context.RecipeIngredients
			.Where(r => r.Id == request.Id)
			.SingleOrDefaultAsync(cancellationToken);

		Guard.Against.NotFound(request.Id, entity);

		context.RecipeIngredients.Remove(entity);

		await context.SaveChangesAsync(cancellationToken);
	}
}
