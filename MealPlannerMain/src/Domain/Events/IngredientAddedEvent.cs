namespace MealPlanner.Domain.Events;

public class IngredientAddedEvent(RecipeIngredient recipeIngredient) : BaseEvent
{
	public RecipeIngredient Item { get; } = recipeIngredient;
}
