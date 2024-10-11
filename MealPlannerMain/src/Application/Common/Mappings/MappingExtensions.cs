using MealPlanner.Application.Common.Models;

namespace MealPlanner.Application.Common.Mappings;

public static class MappingExtensions
{
	public static Task<List<TDestination>> ProjectToListAsync<TDestination>(
		this IQueryable queryable,
		IConfigurationProvider configuration
	)
		where TDestination : class =>
		queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();

	public static List<TDestination> ProjectToList<TSource, TDestination>(
		this IEnumerable<TSource> list,
		IMapper mapper
	)
		where TDestination : class => list.Select(l => mapper.Map<TDestination>(l)).ToList();
}
