using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Application.Common.Mappings;

namespace MealPlanner.Application.Recipes.Queries;

public record GetAllRecipesQuery : IRequest<IReadOnlyCollection<RecipeDto>> { }

public class GetAllRecipesQueryValidator : AbstractValidator<GetAllRecipesQuery>
{
	public GetAllRecipesQueryValidator() { }
}

public class GetAllRecipesQueryHandler(
	IApplicationDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllRecipesQuery, IReadOnlyCollection<RecipeDto>>
{
	public async Task<IReadOnlyCollection<RecipeDto>> Handle(
		GetAllRecipesQuery request,
		CancellationToken cancellationToken
	)
	{
		var recipes = await context
			.Recipes
			.Include(r => r.RecipeIngredients)
			.ThenInclude(ri => ri.Ingredient)
			.ProjectToListAsync<RecipeDto>(mapper.ConfigurationProvider);

		return recipes;
	}
}
