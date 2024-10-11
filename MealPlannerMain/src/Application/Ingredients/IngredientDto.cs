using MealPlanner.Domain.Entities;

namespace MealPlanner.Application.Tenants;

public class IngredientDto
{
	public string Id { get; set; } = null!;
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
