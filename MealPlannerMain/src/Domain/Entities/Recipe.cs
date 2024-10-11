namespace MealPlanner.Domain.Entities;
public class Recipe : BaseAuditableEntity
{
	public required string Name { get; set; }
	public int PeopleFed { get; set; }

	public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = [];
}
