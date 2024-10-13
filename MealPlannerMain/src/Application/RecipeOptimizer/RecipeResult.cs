﻿namespace MealPlanner.Application.RecipeOptimizer;

public class RecipeResult
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null!;
	public int Quantity { get; set; }
	public int PeopleFed { get; set; }
}
