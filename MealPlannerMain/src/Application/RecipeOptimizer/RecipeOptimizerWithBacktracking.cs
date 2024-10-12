using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Application.Ingredients;
using MealPlanner.Domain.Entities;

namespace MealPlanner.Application.RecipeOptimizer;
public class RecipeOptimizerWithBacktracking(IApplicationDbContext context) : IRecipeOptimizerWithBacktracking
{
	private readonly Dictionary<string, int> _memo = [];  // Memoization

	public async Task<CalculationResult> OptimizeFeeding(List<IngredientDto> availableIngredients, CancellationToken cancellationToken)
	{
		var availableIngredientsDict = availableIngredients.ToDictionary(i => i.Id, i => i.Quantity);

		var recipes = await context.Recipes.Include(r => r.RecipeIngredients)
			.ToListAsync(cancellationToken);

		var maxPeopleFed = Backtrack(recipes, availableIngredientsDict);

		return new CalculationResult
		{
			PeopleFed = maxPeopleFed,
			// Optionally return recipe and ingredient usage details
		};
	}

	private int Backtrack(List<Recipe> recipes, Dictionary<Guid, int> availableIngredients)
	{
		var stateKey = GenerateStateKey(availableIngredients);

		if (_memo.TryGetValue(stateKey, out var value))
		{
			return value;
		}

		var maxPeopleFed = 0;

		for (var i = 0; i < recipes.Count; i++) //Brute Force
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
				var peopleFed = recipe.PeopleFed + Backtrack(recipes, newIngredients);

				maxPeopleFed = Math.Max(maxPeopleFed, peopleFed);
			}
		}

		_memo[stateKey] = maxPeopleFed;

		return maxPeopleFed;
	}

	private static string GenerateStateKey(Dictionary<Guid, int> availableIngredients)
	{
		return string.Join(",", availableIngredients.Select(kvp => $"{kvp.Key}:{kvp.Value}"));
	}
}
