using System.Reflection;
using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
	: DbContext(options),
		IApplicationDbContext,
		IApplicationDatabaseService
{
	public DbSet<Ingredient> Ingredients => Set<Ingredient>();
	public DbSet<Recipe> Recipes => Set<Recipe>();
	public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
	public DbSet<CalculationHistory> CalculationHistories => Set<CalculationHistory>();
	public DbSet<CalculationRecipeDetail> CalculationRecipeDetails => Set<CalculationRecipeDetail>();
	public DbSet<CalculationIngredientDetail> CalculationIngredientDetails => Set<CalculationIngredientDetail>();

	public DbSet<User> Users => Set<User>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	public async Task ExecuteSqlCommandAsync(string sql, params object[] parameters)
	{
		await Database.ExecuteSqlRawAsync(sql, parameters);
	}
}
