using MealPlanner.Application.Ingredients;
using MealPlanner.Application.RecipeOptimizer;

namespace MealPlanner.Application.Common.Interfaces;
public interface IRecipeOptimizerWithBacktracking
{
	Task<CalculationResult> OptimizeFeeding(List<IngredientDto> availableIngredients, CancellationToken cancellationToken);
}
