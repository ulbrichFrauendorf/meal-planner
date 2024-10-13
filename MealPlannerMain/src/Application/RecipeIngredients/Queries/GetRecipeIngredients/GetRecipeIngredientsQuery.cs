using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Application.Common.Mappings;
using MealPlanner.Application.Common.Security;
using MealPlanner.Domain.Constants;

namespace MealPlanner.Application.RecipeIngredients.Queries.GetRecipeIngredients;

[Authorize(Policy = Policies.Admin)]
public record GetRecipeIngredientsQuery : IRequest<IReadOnlyCollection<RecipeIngredientDto>>
{
	public Guid RecipeId { get; init; }
}

public class GetRecipeIngredientsQueryHandler(
	IApplicationDbContext context,
	IMapper mapper
) : IRequestHandler<GetRecipeIngredientsQuery, IReadOnlyCollection<RecipeIngredientDto>>
{
	public async Task<IReadOnlyCollection<RecipeIngredientDto>> Handle(
		GetRecipeIngredientsQuery request,
		CancellationToken cancellationToken
	)
	{
		var recipeIngredients = await context.RecipeIngredients
			.Include(r => r.Ingredient)
			.Where(re => re.RecipeId == request.RecipeId)
			.ProjectToListAsync<RecipeIngredientDto>(mapper.ConfigurationProvider);

		return recipeIngredients;
	}
}
