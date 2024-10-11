namespace MealPlanner.Domain.Entities;
public class CalculationHistory : BaseAuditableEntity
{
	public DateTime CalculationDate { get; set; }
	public int PeopleFed { get; set; }

	public ICollection<CalculationRecipeDetail> RecipeDetails { get; set; } = [];
	public ICollection<CalculationIngredientDetail> IngredientDetails { get; set; } = [];
}
