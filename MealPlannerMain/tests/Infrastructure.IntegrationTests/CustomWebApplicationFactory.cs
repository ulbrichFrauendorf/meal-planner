using Invensys.Api.PaySpace;
using MealPlanner.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MealPlanner.Infrastructure.IntegrationTests;

public class CustomWebApplicationFactory() : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureAppConfiguration(config =>
		{
			config.AddJsonFile("appsettings.json").AddEnvironmentVariables();
			config.AddJsonFile("appsettings.development.json").AddEnvironmentVariables();
			config.AddUserSecrets<Program>();
		});

		builder.ConfigureTestServices(services =>
		{
			services.AddHttpClient();
			services.AddPaySpaceApiServices();
		});
	}
}
