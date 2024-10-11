using System.Security.Claims;
using MealPlanner.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace MealPlanner.Infrastructure.Identity;

public class IdentityService(
	IHttpContextAccessor httpContextAccessor,
	IAuthorizationService authorizationService
) : IIdentityService
{
	public string? GetUserId()
	{
		return GetUser()?.FindFirstValue(ClaimTypes.NameIdentifier);
	}

	public string? GetUserEmail()
	{
		return GetUser()?.FindFirstValue(ClaimTypes.Email);
	}

	public string? GetUserName()
	{
		return GetUser()?.FindFirstValue(ClaimTypes.Email);
	}

	public bool IsInRole(string role)
	{
		var user = GetUser();

		return user != null && user!.HasClaim(ClaimTypes.Role, role);
	}

	public bool HasSystemClaim(string claim)
	{
		var user = GetUser();

		return user != null && user!.HasClaim(ClaimTypes.System, claim);
	}

	public List<string> GetUserClaims()
	{
		var user = GetUser();

		if (user == null)
		{
			return [];
		}

		var claims = user!.Claims ?? [];

		return claims.Select(c => c.Value).ToList();
	}

	public async Task<bool> AuthorizeAsync(string policy)
	{
		var user = GetUser();

		if (user == null)
		{
			return false;
		}

		var result = await authorizationService.AuthorizeAsync(user!, policy);

		return result.Succeeded;
	}

	private ClaimsPrincipal? GetUser()
	{
		var user = httpContextAccessor.HttpContext?.User;

		return user;
	}
}
