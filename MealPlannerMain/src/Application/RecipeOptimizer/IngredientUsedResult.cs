namespace MealPlanner.Application.RecipeOptimizer;

public class IngredientUsedResult
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null!;
	public int Quantity { get; set; }
}
