using MealPlanner.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MealPlanner.Infrastructure.Data;

public static class InitialiserExtensions
{
	public static async Task InitialiseDatabaseAsync(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();

		var initialiser =
			scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

		await initialiser.InitialiseAsync();

		await initialiser.SeedAsync();
	}
}

public class ApplicationDbContextInitialiser(
	ILogger<ApplicationDbContextInitialiser> logger,
	ApplicationDbContext context
)
{
	public async Task InitialiseAsync()
	{
		try
		{
			await context.Database.MigrateAsync();
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred while initialising the database.");
			throw;
		}
	}

	public async Task SeedAsync()
	{
		try
		{
			await TrySeedAsync();
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred while seeding the database.");
			throw;
		}
	}

	public async Task TrySeedAsync()
	{
		//// Default data
		//// Seed, if necessary
		//Guid userId;

		//if (!context.Users.Any(u => u.Email == Constants.TestUserEmail))
		//{
		//	await context.Users.AddRangeAsync(
		//		[new User { Id = Constants.TestUserId, Email = Constants.TestUserEmail }]
		//	);
		//	await context.SaveChangesAsync();
		//}
		//else
		//{
		//	userId = context.Users.First(s => s.Email == Constants.TestUserEmail).Id;
		//}

		if (!context.Ingredients.Any())
		{
			await context.Ingredients.AddRangeAsync(
				new Ingredient { Name = "Cucumber", Quantity = 2 },
				new Ingredient { Name = "Olives", Quantity = 2 },
				new Ingredient { Name = "Lettuce", Quantity = 3 },
				new Ingredient { Name = "Meat", Quantity = 6 },
				new Ingredient { Name = "Tomato", Quantity = 6 },
				new Ingredient { Name = "Cheese", Quantity = 8 },
				new Ingredient { Name = "Dough", Quantity = 10 }
			);
			await context.SaveChangesAsync();
		}

		if (!context.Recipes.Any())
		{
			await context.Recipes.AddRangeAsync(
				new Recipe { Name = "Pizza", PeopleFed = 4 },
				new Recipe { Name = "Salad", PeopleFed = 3 },
				new Recipe { Name = "Pasta", PeopleFed = 2 },
				new Recipe { Name = "Burger", PeopleFed = 1 },
				new Recipe { Name = "Pie", PeopleFed = 1 },
				new Recipe { Name = "Sandwich", PeopleFed = 1 }
			);
			await context.SaveChangesAsync();
		}

		if (!context.RecipeIngredients.Any())
		{
			var pizza = context.Recipes.First(r => r.Name == "Pizza").Id;
			var salad = context.Recipes.First(r => r.Name == "Salad").Id;
			var pasta = context.Recipes.First(r => r.Name == "Pasta").Id;
			var burger = context.Recipes.First(r => r.Name == "Burger").Id;
			var pie = context.Recipes.First(r => r.Name == "Pie").Id;
			var sandwich = context.Recipes.First(r => r.Name == "Sandwich").Id;

			var dough = context.Ingredients.First(i => i.Name == "Dough").Id;
			var tomato = context.Ingredients.First(i => i.Name == "Tomato").Id;
			var cheese = context.Ingredients.First(i => i.Name == "Cheese").Id;
			var olives = context.Ingredients.First(i => i.Name == "Olives").Id;
			var meat = context.Ingredients.First(i => i.Name == "Meat").Id;
			var cucumber = context.Ingredients.First(i => i.Name == "Cucumber").Id;
			var lettuce = context.Ingredients.First(i => i.Name == "Lettuce").Id;

			await context.RecipeIngredients.AddRangeAsync(
				// Pizza
				new RecipeIngredient { RecipeId = pizza, IngredientId = dough, Quantity = 3 },
				new RecipeIngredient { RecipeId = pizza, IngredientId = tomato, Quantity = 2 },
				new RecipeIngredient { RecipeId = pizza, IngredientId = cheese, Quantity = 2 },
				new RecipeIngredient { RecipeId = pizza, IngredientId = olives, Quantity = 1 },

				// Salad
				new RecipeIngredient { RecipeId = salad, IngredientId = lettuce, Quantity = 2 },
				new RecipeIngredient { RecipeId = salad, IngredientId = tomato, Quantity = 2 },
				new RecipeIngredient { RecipeId = salad, IngredientId = cucumber, Quantity = 1 },
				new RecipeIngredient { RecipeId = salad, IngredientId = cheese, Quantity = 2 },
				new RecipeIngredient { RecipeId = salad, IngredientId = olives, Quantity = 1 },

				// Pasta
				new RecipeIngredient { RecipeId = pasta, IngredientId = dough, Quantity = 2 },
				new RecipeIngredient { RecipeId = pasta, IngredientId = tomato, Quantity = 1 },
				new RecipeIngredient { RecipeId = pasta, IngredientId = cheese, Quantity = 1 },
				new RecipeIngredient { RecipeId = pasta, IngredientId = meat, Quantity = 1 },

				// Burger
				new RecipeIngredient { RecipeId = burger, IngredientId = meat, Quantity = 1 },
				new RecipeIngredient { RecipeId = burger, IngredientId = lettuce, Quantity = 1 },
				new RecipeIngredient { RecipeId = burger, IngredientId = tomato, Quantity = 1 },
				new RecipeIngredient { RecipeId = burger, IngredientId = cheese, Quantity = 1 },
				new RecipeIngredient { RecipeId = burger, IngredientId = dough, Quantity = 1 },

				// Pie
				new RecipeIngredient { RecipeId = pie, IngredientId = dough, Quantity = 2 },
				new RecipeIngredient { RecipeId = pie, IngredientId = meat, Quantity = 2 },

				// Sandwich
				new RecipeIngredient { RecipeId = sandwich, IngredientId = dough, Quantity = 1 },
				new RecipeIngredient { RecipeId = sandwich, IngredientId = cucumber, Quantity = 1 }
			);

			await context.SaveChangesAsync();
		}

		if (!context.CalculationHistories.Any())
		{
			var testHistory = new CalculationHistory
			{
				CalculationDate = DateTime.Now,
				PeopleFed = 12,
				RecipeDetails = new List<CalculationRecipeDetail>
			{
				new CalculationRecipeDetail
				{
					RecipeId = context.Recipes.First(r => r.Name == "Pizza").Id,
					QuantityMade = 2
				},
				new CalculationRecipeDetail
				{
					RecipeId = context.Recipes.First(r => r.Name == "Pasta").Id,
					QuantityMade = 2
				}
			},
				IngredientDetails = new List<CalculationIngredientDetail>
			{
				new CalculationIngredientDetail
				{
					IngredientId = context.Ingredients.First(i => i.Name == "Dough").Id,
					QuantityUsed = 10
				},
				new CalculationIngredientDetail
				{
					IngredientId = context.Ingredients.First(i => i.Name == "Tomato").Id,
					QuantityUsed = 6
				},
				new CalculationIngredientDetail
				{
					IngredientId = context.Ingredients.First(i => i.Name == "Cheese").Id,
					QuantityUsed = 6
				},
				new CalculationIngredientDetail
				{
					IngredientId = context.Ingredients.First(i => i.Name == "Olives").Id,
					QuantityUsed = 2
				},
				new CalculationIngredientDetail
				{
					IngredientId = context.Ingredients.First(i => i.Name == "Meat").Id,
					QuantityUsed = 2
				}
			}
			};

			await context.CalculationHistories.AddAsync(testHistory);
			await context.SaveChangesAsync();
		}
	}
}
