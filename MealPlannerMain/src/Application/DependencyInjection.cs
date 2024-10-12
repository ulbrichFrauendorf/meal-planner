using System.Reflection;
using MealPlanner.Application.Common.Behaviours;
using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Application.RecipeOptimizer;
using Microsoft.Extensions.DependencyInjection;

namespace MealPlanner.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddAutoMapper(Assembly.GetExecutingAssembly());

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
		});

		services.AddTransient<IRecipeOptimizerWithBacktracking, RecipeOptimizerWithBacktracking>();

		return services;
	}
}
