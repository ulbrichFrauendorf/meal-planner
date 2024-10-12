using MealPlanner.Domain.Entities;

namespace MealPlanner.Application.Ingredients;

public class IngredientDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null!;
	public int Quantity { get; set; }

	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<Ingredient, IngredientDto>();
		}
	}
}
