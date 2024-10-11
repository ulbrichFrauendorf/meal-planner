using MealPlanner.Domain.Entities;

namespace MealPlanner.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	DbSet<Ingredient> Ingredients { get; }
	DbSet<Recipe> Recipes { get; }
	DbSet<RecipeIngredient> RecipeIngredients { get; }
	DbSet<CalculationHistory> CalculationHistories { get; }
	DbSet<CalculationRecipeDetail> CalculationRecipeDetails { get; }
	DbSet<CalculationIngredientDetail> CalculationIngredientDetails { get; }
	DbSet<User> Users { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
