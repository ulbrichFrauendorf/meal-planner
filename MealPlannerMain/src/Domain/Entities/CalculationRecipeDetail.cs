namespace MealPlanner.Domain.Entities;
public class CalculationRecipeDetail : BaseAuditableEntity
{
	public Guid CalculationHistoryId { get; set; }
	public CalculationHistory CalculationHistory { get; set; } = null!;

	public Guid RecipeId { get; set; }
	public Recipe Recipe { get; set; } = null!;

	public int QuantityMade { get; set; }
}
