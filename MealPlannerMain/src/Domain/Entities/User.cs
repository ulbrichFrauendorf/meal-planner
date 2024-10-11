namespace MealPlanner.Domain.Entities;

public class User : BaseAuditableEntity
{
	public required string Email { get; set; }
	public IList<string> Claims { get; set; } = new List<string>();

}
