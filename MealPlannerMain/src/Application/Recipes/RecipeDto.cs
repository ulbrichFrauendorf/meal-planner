using MealPlanner.Domain.Entities;

namespace MealPlanner.Application.Recipes;

public class RecipeDto
{
	public string Id { get; set; } = null!;
	public string Name { get; set; } = null!;
	public int PeopleFed { get; set; }
	public IReadOnlyCollection<RecipeIngredientDto> RecipeIngredients { get; set; } = [];

	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<Recipe, RecipeDto>();
		}
	}
}
