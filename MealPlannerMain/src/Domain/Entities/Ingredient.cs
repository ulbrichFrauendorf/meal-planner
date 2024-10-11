namespace MealPlanner.Domain.Entities;
public class Ingredient : BaseAuditableEntity
{
	public required string Name { get; set; }
	public int Quantity { get; set; }

	public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = [];
}
