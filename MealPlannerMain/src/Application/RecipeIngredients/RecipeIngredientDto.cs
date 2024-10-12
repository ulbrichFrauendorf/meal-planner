using MealPlanner.Domain.Entities;

namespace MealPlanner.Application.RecipeIngredients;

public class RecipeIngredientDto
{
	public string Id { get; set; } = null!;
	public string IngredientName { get; set; } = null!;
	public int Quantity { get; set; }

	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<RecipeIngredient, RecipeIngredientDto>()
				.ForMember(dest => dest.IngredientName, cfg => cfg.MapFrom(src => src.Ingredient.Name));

		}
	}
}
