using MealPlanner.Domain.Events;
using Microsoft.Extensions.Logging;

namespace MealPlanner.Application.RecipeIngredients.Commands.AddRecipeIngredient;

public class AddRecipeIngredientEventHandler(ILogger<AddRecipeIngredientEventHandler> logger) : INotificationHandler<IngredientAddedEvent>
{
	public Task Handle(IngredientAddedEvent notification, CancellationToken cancellationToken)
	{
		logger.LogInformation("MealPlanner Domain Event: {DomainEvent}", notification.GetType().Name);

		return Task.CompletedTask;
	}
}
