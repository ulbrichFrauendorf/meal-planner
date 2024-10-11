using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Application.Common.Mappings;

namespace MealPlanner.Application.Tenants.Queries;

public record GetAllIngredientsQuery : IRequest<IReadOnlyCollection<IngredientDto>> { }

public class GetAllIngredientsQueryValidator : AbstractValidator<GetAllIngredientsQuery>
{
	public GetAllIngredientsQueryValidator() { }
}

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
