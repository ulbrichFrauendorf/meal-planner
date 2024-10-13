using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Application.Ingredients;
using MealPlanner.Domain.Entities;

namespace MealPlanner.Application.RecipeOptimizer;
namespace MealPlanner.Application.RecipeOptimizer;

public class RecipeOptimizerWithBacktracking(IApplicationDbContext context) : IRecipeOptimizerWithBacktracking
{
	private readonly Dictionary<string, (int peopleFed, List<RecipeResult> recipesUsed, Dictionary<Guid, int> ingredientsUsed)> _memo = new(); // Memoization

	public async Task<CalculationResult> OptimizeFeeding(List<IngredientDto> availableIngredients, CancellationToken cancellationToken)
	{
		var availableIngredientsDict = availableIngredients.ToDictionary(i => i.Id, i => i.Quantity);

		var recipes = await context.Recipes.Include(r => r.RecipeIngredients)
			.ToListAsync(cancellationToken);

		var (maxPeopleFed, recipeResults, ingredientsUsed) = Backtrack(recipes, availableIngredientsDict);

		// Group the recipe results
		var groupedRecipeResults = recipeResults
			.GroupBy(r => r.Name)
			.Select(g => new RecipeResult
			{
				Id = g.First().Id, // Assuming all recipes with the same name have the same ID
				Name = g.Key,
				Quantity = g.Count(),
				PeopleFed = g.Sum(r => r.PeopleFed)
			})
			.ToList();

		// Create the list of ingredients used based on the final bestIngredientsUsed dictionary
		var ingredientsUsedResults = ingredientsUsed
			.Select(kvp => new IngredientUsedResult
			{
				Id = kvp.Key,
				Name = availableIngredients.First(i => i.Id == kvp.Key).Name,
				Quantity = kvp.Value
			})
			.ToList();

		return new CalculationResult
		{
			PeopleFed = maxPeopleFed,
			Recipes = groupedRecipeResults,
			IngredientsUsed = ingredientsUsedResults
		};
	}

	private (int peopleFed, List<RecipeResult> recipesUsed, Dictionary<Guid, int> ingredientsUsed) Backtrack(List<Recipe> recipes, Dictionary<Guid, int> availableIngredients)
	{
		var stateKey = GenerateStateKey(availableIngredients);

		if (_memo.TryGetValue(stateKey, out var memoizedResult))
		{
			return (memoizedResult.peopleFed, memoizedResult.recipesUsed, memoizedResult.ingredientsUsed);
		}

		var maxPeopleFed = 0;
		var bestRecipeCombination = new List<RecipeResult>();
		var bestIngredientsUsed = new Dictionary<Guid, int>();

		for (var i = 0; i < recipes.Count; i++) // Brute Force
		{
			var recipe = recipes[i];
			var canMakeRecipe = true;

			var newIngredients = new Dictionary<Guid, int>(availableIngredients);
			var currentIngredientsUsed = new Dictionary<Guid, int>();

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

					if (currentIngredientsUsed.ContainsKey(ingredient.IngredientId))
					{
						currentIngredientsUsed[ingredient.IngredientId] += ingredient.Quantity;
					}
					else
					{
						currentIngredientsUsed[ingredient.IngredientId] = ingredient.Quantity;
					}
				}
				else
				{
					canMakeRecipe = false;
					break;
				}
			}

			if (canMakeRecipe)
			{
				var (peopleFedWithCurrentRecipe, recipesUsedForCurrentCombo, ingredientsUsedForCurrentCombo) = Backtrack(recipes, newIngredients);

				var totalPeopleFed = recipe.PeopleFed + peopleFedWithCurrentRecipe;

				if (totalPeopleFed > maxPeopleFed)
				{
					maxPeopleFed = totalPeopleFed;

					bestRecipeCombination = new List<RecipeResult>(recipesUsedForCurrentCombo)
					{
						new RecipeResult
						{
							Id = recipe.Id,
							Name = recipe.Name,
							Quantity = 1,
							PeopleFed = recipe.PeopleFed
						}
					};

					bestIngredientsUsed = new Dictionary<Guid, int>(ingredientsUsedForCurrentCombo);
					foreach (var kvp in currentIngredientsUsed)
					{
						if (bestIngredientsUsed.ContainsKey(kvp.Key))
						{
							bestIngredientsUsed[kvp.Key] += kvp.Value;
						}
						else
						{
							bestIngredientsUsed[kvp.Key] = kvp.Value;
						}
					}
				}
			}
		}

		_memo[stateKey] = (maxPeopleFed, bestRecipeCombination, bestIngredientsUsed);

		return (maxPeopleFed, bestRecipeCombination, bestIngredientsUsed);
	}

	private static string GenerateStateKey(Dictionary<Guid, int> availableIngredients)
	{
		return string.Join(",", availableIngredients.Select(kvp => $"{kvp.Key}:{kvp.Value}"));
	}
}

