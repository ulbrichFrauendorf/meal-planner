using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Application.Ingredients;
using MealPlanner.Domain.Entities;

namespace MealPlanner.Application.RecipeOptimizer;
public class RecipeOptimizerWithBacktracking(IApplicationDbContext context) : IRecipeOptimizerWithBacktracking
{
	private readonly Dictionary<string, (int peopleFed, List<RecipeResult> recipesUsed)> _memo = []; // Memoization

	public async Task<CalculationResult> OptimizeFeeding(List<IngredientDto> availableIngredients, CancellationToken cancellationToken)
	{
		var availableIngredientsDict = availableIngredients.ToDictionary(i => i.Id, i => i.Quantity);

		var recipes = await context.Recipes.Include(r => r.RecipeIngredients)
			.ToListAsync(cancellationToken);

		var (maxPeopleFed, recipeResults) = Backtrack(recipes, availableIngredientsDict);

		var groupedRecipeReslts = recipeResults
			.GroupBy(r => r.Name)
			.Select(g => new RecipeResult { Name = g.Key, Quantity = g.Count(), PeopleFed = g.Sum(r => r.PeopleFed) })
			.ToList();

		return new CalculationResult { PeopleFed = maxPeopleFed, Recipes = groupedRecipeReslts };
	}

	private (int peopleFed, List<RecipeResult> recipesUsed) Backtrack(List<Recipe> recipes, Dictionary<Guid, int> availableIngredients)
	{
		var stateKey = GenerateStateKey(availableIngredients);

		if (_memo.TryGetValue(stateKey, out var memoizedResult))
		{
			return memoizedResult;
		}

		var maxPeopleFed = 0;
		var bestRecipeCombination = new List<RecipeResult>();

		for (var i = 0; i < recipes.Count; i++) // Brute Force
		{
			var recipe = recipes[i];
			var canMakeRecipe = true;

			var newIngredients = new Dictionary<Guid, int>(availableIngredients);

			foreach (var ingredient in recipe.RecipeIngredients)
			{
				if (newIngredients.ContainsKey(ingredient.IngredientId))
				{
					var availableQuantity = newIngredients[ingredient.IngredientId];

					if (availableQuantity < ingredient.Quantity)
					{
						canMakeRecipe = false;
						break;
					}

					newIngredients[ingredient.IngredientId] -= ingredient.Quantity;
				}
				else
				{
					canMakeRecipe = false;
					break;
				}
			}

			if (canMakeRecipe)
			{
				// Recursively find the best combination using remaining ingredients
				var (peopleFedWithCurrentRecipe, recipesUsedForCurrentCombo) = Backtrack(recipes, newIngredients);

				var totalPeopleFed = recipe.PeopleFed + peopleFedWithCurrentRecipe;

				if (totalPeopleFed > maxPeopleFed)
				{
					maxPeopleFed = totalPeopleFed;

					bestRecipeCombination = new List<RecipeResult>(recipesUsedForCurrentCombo)
					{
						new RecipeResult { Name = recipe.Name, PeopleFed = recipe.PeopleFed }
					};
				}
			}
		}

		_memo[stateKey] = (maxPeopleFed, bestRecipeCombination);

		return (maxPeopleFed, bestRecipeCombination);
	}

	private static string GenerateStateKey(Dictionary<Guid, int> availableIngredients)
	{
		return string.Join(",", availableIngredients.Select(kvp => $"{kvp.Key}:{kvp.Value}"));
	}
}
