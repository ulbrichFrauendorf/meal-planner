namespace MealPlanner.Domain.Events;

public class IngredientAddedEvent(Ingredient ingredient) : BaseEvent
{
	public Ingredient Item { get; } = ingredient;
}
