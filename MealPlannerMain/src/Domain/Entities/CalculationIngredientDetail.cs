namespace MealPlanner.Domain.Entities;
public class CalculationIngredientDetail : BaseAuditableEntity
{
	public Guid CalculationHistoryId { get; set; }
	public CalculationHistory CalculationHistory { get; set; } = null!;

	public Guid IngredientId { get; set; }
	public Ingredient Ingredient { get; set; } = null!;

	public int QuantityUsed { get; set; }
}
