namespace MealPlanner.Application.RecipeOptimizer;

public class CalculationResult
{
	public int PeopleFed { get; set; }
	public List<RecipeResult> Recipes { get; set; } = [];
	public List<IngredientUsedResult> IngredientsUsed { get; set; } = [];
}
