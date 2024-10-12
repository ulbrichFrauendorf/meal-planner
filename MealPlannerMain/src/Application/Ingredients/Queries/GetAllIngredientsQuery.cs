using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Application.Common.Mappings;

namespace MealPlanner.Application.Ingredients.Queries;

public record GetAllIngredientsQuery : IRequest<IReadOnlyCollection<IngredientDto>> { }

public class GetAllIngredientsQueryHandler(
	IApplicationDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllIngredientsQuery, IReadOnlyCollection<IngredientDto>>
{
	public async Task<IReadOnlyCollection<IngredientDto>> Handle(
		GetAllIngredientsQuery request,
		CancellationToken cancellationToken
	)
	{
		var ingredients = await context
			.Ingredients
			.ProjectToListAsync<IngredientDto>(mapper.ConfigurationProvider);

		return ingredients;
	}
}
