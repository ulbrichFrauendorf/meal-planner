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

		services.AddRazorPages();

		// Customise default API behaviour
		services.Configure<ApiBehaviorOptions>(options =>
			options.SuppressModelStateInvalidFilter = true
		);

		services.AddEndpointsApiExplorer();

		services.AddOpenApiDocument(
			(configure, sp) =>
			{
				configure.Title = "Iserve Reporting API";
				configure.PostProcess = document =>
				{
					document.Info = new OpenApiInfo
					{
						Version = "v1",
						Title = "ToDo API",
						Description = "An ASP.NET Core Web API for managing ToDo items",
						TermsOfService = "https://example.com/terms",
						Contact = new OpenApiContact
						{
							Name = "Example Contact",
							Url = "https://example.com/contact"
						},
						License = new OpenApiLicense
						{
							Name = "Example License",
							Url = "https://example.com/license"
						}
					};
				};

				configure.AddSecurity(
					"JWT Token",
					Enumerable.Empty<string>(),
					new OpenApiSecurityScheme
					{
						Description =
							"JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
						Type = OpenApiSecuritySchemeType.ApiKey,
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
