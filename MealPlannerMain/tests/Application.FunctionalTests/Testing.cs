using System.Security.Claims;
using MealPlanner.Domain.Constants;
using MealPlanner.Infrastructure.Data;
using MealPlanner.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MealPlanner.Application.FunctionalTests;

[SetUpFixture]
public partial class Testing
{
	private static ITestDatabase _database;
	private static CustomWebApplicationFactory _factory = null!;
	private static IServiceScopeFactory _scopeFactory = null!;
	private static string? _userId;

	[OneTimeSetUp]
	public async Task RunBeforeAnyTests()
	{
		_database = await TestDatabaseFactory.CreateAsync();

		_factory = new CustomWebApplicationFactory(_database.GetConnection());

		_scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
	}

	public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
	{
		using var scope = _scopeFactory.CreateScope();

		var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

		return await mediator.Send(request);
	}

	public static async Task SendAsync(IBaseRequest request)
	{
		using var scope = _scopeFactory.CreateScope();

		var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

		await mediator.Send(request);
	}

	public static string? GetUserId()
	{
		return _userId;
	}

	public static string RunAsDefaultUserAsync()
	{
		return RunAsUserAsync("test@local", "Testing1234!", Array.Empty<string>());
	}

	public static string RunAsAdministratorAsync()
	{
		return RunAsUserAsync(
			"administrator@local",
			"Administrator1234!",
			new[] { Roles.Administrator }
		);
	}

	public static string RunAsUserAsync(string userName, string password, string[] roles)
	{
		var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

		var context = new DefaultHttpContext();

		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
			new Claim(ClaimTypes.Email, userName),
			new Claim(ClaimTypes.Name, userName),
			new Claim(ClaimTypes.Role, "TestRole")
		};

		var identity = new ClaimsIdentity(claims, "TestAuthType");

		var principal = new ClaimsPrincipal(identity);

		context.User = principal;

		mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

		using var scope = _scopeFactory.CreateScope();

		var authService = scope.ServiceProvider.GetRequiredService<IAuthorizationService>();

		var identityService = new IdentityService(mockHttpContextAccessor.Object, authService);

		Guard.Against.Null(identityService);

		_userId = identityService.GetUserId();

		return _userId!.ToString();
	}

	public static async Task ResetState()
	{
		try
		{
			await _database.ResetAsync();
		}
		catch (Exception) { }

		_userId = null;
	}

	public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
		where TEntity : class
	{
		using var scope = _scopeFactory.CreateScope();

		var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		return await context.FindAsync<TEntity>(keyValues);
	}

	public static async Task AddAsync<TEntity>(TEntity entity)
		where TEntity : class
	{
		using var scope = _scopeFactory.CreateScope();

		var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		context.Add(entity);

		await context.SaveChangesAsync();
	}

	public static async Task<int> CountAsync<TEntity>()
		where TEntity : class
	{
		using var scope = _scopeFactory.CreateScope();

		var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		return await context.Set<TEntity>().CountAsync();
	}

	[OneTimeTearDown]
	public async Task RunAfterAnyTests()
	{
		await _database.DisposeAsync();
		await _factory.DisposeAsync();
	}
}
