using System.Data.Common;
using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Infrastructure.Data;
using MealPlanner.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using static MealPlanner.Application.FunctionalTests.Testing;

namespace MealPlanner.Application.FunctionalTests;

public class CustomWebApplicationFactory(DbConnection connection) : WebApplicationFactory<Program>
{
	private readonly DbConnection _connection = connection;

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureTestServices(services =>
		{
			services
				.RemoveAll<IUser>()
				.AddTransient(provider => Mock.Of<IUser>(s => s.Id == GetUserId()));

			services
				.RemoveAll<DbContextOptions<ApplicationDbContext>>()
				.AddDbContext<ApplicationDbContext>(
					(sp, options) =>
					{
						options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

						options.UseSqlServer(_connection);
					}
				);
		});
	}
}
