using System.Security.Claims;
using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Domain.Constants;
using MealPlanner.Infrastructure.Config;
using MealPlanner.Infrastructure.Data;
using MealPlanner.Infrastructure.Data.Interceptors;
using MealPlanner.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Sentry.OpenTelemetry;

namespace MealPlanner.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructureServices(
		this IServiceCollection services,
		IConfiguration configuration,
		Microsoft.AspNetCore.Hosting.IWebHostEnvironment environment
	)
	{
		services.AddMemoryCache();

		var connectionString = configuration.GetConnectionString("DefaultConnection");

		Guard.Against.Null(
			connectionString,
			message: "Connection string 'DefaultConnection' not found."
		);

		services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

		services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

		services.AddDbContext<ApplicationDbContext>(
			(sp, options) =>
			{
				options.ConfigureWarnings(w => w.Throw());
				options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

				options.UseSqlServer(connectionString);
			}
		);

		services.AddScoped<IApplicationDbContext>(provider =>
			provider.GetRequiredService<ApplicationDbContext>()
		);

		services.AddScoped<ApplicationDbContextInitialiser>();

		services.AddScoped<IApplicationDatabaseService>(provider =>
			provider.GetRequiredService<ApplicationDbContext>()
		);

		var jwtConfig = configuration.GetSection(nameof(JwtConfig)).Get<JwtConfig>();

		services
			.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.Authority = jwtConfig!.Issuer;
				options.Audience = jwtConfig!.Audience;
				options.RequireHttpsMetadata = true;

				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true
				};
			});

		services.AddAuthorizationBuilder()
			.AddPolicy(Policies.Admin, policy =>
				{
					policy.RequireAuthenticatedUser();
					policy.RequireClaim(ClaimTypes.Role, Policies.Admin);
					policy.RequireClaim(ClaimTypes.System, Policies.SystemAccess);
				})
			.AddPolicy(Policies.User, policy =>
				{
					policy.RequireAuthenticatedUser();
					policy.RequireClaim(ClaimTypes.Role, Policies.User);
					policy.RequireClaim(ClaimTypes.System, Policies.SystemAccess);
				});

		services.AddSingleton(TimeProvider.System);
		services.AddTransient<IIdentityService, IdentityService>();

		services.AddCors(options =>
		{
			options.AddPolicy("AllowAllMethods", options => options.AllowAnyMethod());
			options.AddPolicy(
				"AllowAngularOrigin",
				builder => builder
						.WithOrigins("https://localhost:44447")
						.AllowAnyHeader()
						.AllowAnyMethod());
		});

		var otel = services.AddOpenTelemetry();

		otel.ConfigureResource(resource =>
			resource.AddService(serviceName: environment.ApplicationName)
		);

		otel.WithTracing(tracing =>
			tracing.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddSentry()
		);

		return services;
	}
}
