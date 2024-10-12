using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Application.Ingredients;

namespace MealPlanner.Application.RecipeOptimizer.Queries;

public record GetOptimalRecipeInformationQuery : IRequest<CalculationResult>
{
	public List<IngredientDto> AvailableIngredients { get; init; } = [];
}

public class GetOptimalRecipeInformationQueryHandler(IRecipeOptimizerWithBacktracking recipeOptimizer)
	: IRequestHandler<GetOptimalRecipeInformationQuery, CalculationResult>
{
	public async Task<CalculationResult> Handle(
		GetOptimalRecipeInformationQuery request,
		CancellationToken cancellationToken
	)
	{
		return await recipeOptimizer.OptimizeFeeding(request.AvailableIngredients, cancellationToken);
	}
}
