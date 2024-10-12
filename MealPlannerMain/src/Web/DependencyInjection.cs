using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Infrastructure.Data;
using MealPlanner.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace MealPlanner.Web;

public static class DependencyInjection
{
	public static IServiceCollection AddWebServices(this IServiceCollection services)
	{
		services.AddDatabaseDeveloperPageExceptionFilter();

		services.AddHttpClient();

		services.AddHttpContextAccessor();

		services.AddScoped<IUser, CurrentUser>();

		services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

		services.AddExceptionHandler<CustomExceptionHandler>();

		// Customise default API behaviour
		services.Configure<ApiBehaviorOptions>(options =>
			options.SuppressModelStateInvalidFilter = true
		);

		services.AddEndpointsApiExplorer();

		services.AddOpenApiDocument(
			(configure, sp) =>
			{
				configure.Title = "Meal Planner API";
				configure.PostProcess = document =>
				{
					document.Info = new OpenApiInfo
					{
						Version = "v1",
						Title = "Meal Planner API",
						Description = "An ASP.NET Core Web API ",
						Contact = new OpenApiContact
						{
							Name = "Ulbrich Frauendorf",
							Url = "https://ulbrichfrauendorf.github.io/cv/"
						}
					};
				};

				configure.AddSecurity(
					"JWT Token",
					[],
					new OpenApiSecurityScheme
					{
						Description =
							"JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
						Type = OpenApiSecuritySchemeType.OpenIdConnect,
						Name = "Authorization",
						In = OpenApiSecurityApiKeyLocation.Header
					}
				);

				configure.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT Token"));
			}
		);

		return services;
	}
}
