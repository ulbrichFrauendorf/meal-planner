using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Application.Common.Mappings;

namespace MealPlanner.Application.Ingredients.Queries;

public record GetAvailableIngredientsQuery : IRequest<IReadOnlyCollection<IngredientDto>> { }

public class GetAvailableIngredientsQueryHandler(
	IApplicationDbContext context,
	IMapper mapper
) : IRequestHandler<GetAvailableIngredientsQuery, IReadOnlyCollection<IngredientDto>>
{
	public async Task<IReadOnlyCollection<IngredientDto>> Handle(
		GetAvailableIngredientsQuery request,
		CancellationToken cancellationToken
	)
	{
		var ingredients = await context
			.Ingredients.Where(i => i.Quantity > 0)
			.ProjectToListAsync<IngredientDto>(mapper.ConfigurationProvider);

		return ingredients;
	}
}
