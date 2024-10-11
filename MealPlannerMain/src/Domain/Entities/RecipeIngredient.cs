namespace MealPlanner.Domain.Entities;
public class RecipeIngredient : BaseAuditableEntity
{
	public Guid RecipeId { get; set; }
	public Recipe Recipe { get; set; } = null!;

	public Guid IngredientId { get; set; }
	public Ingredient Ingredient { get; set; } = null!;

	public int Quantity { get; set; }
}
