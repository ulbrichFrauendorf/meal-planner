using System.Security.Claims;
using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Application.Common.Models;
using invensys.iserve.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace invensys.iserve.Infrastructure.Identity;

public class IdentitySystemClaimService(
   UserManager<ApplicationUser> userManager,
   IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
   : IdentityService(userManager), IIdentitySystemClaimService
{
   public async Task<List<Claim>> GetSystemClaimsAsync(string userId)
   {
      var user = await GetUserById(userId);

      var claims = await UserManager.GetClaimsAsync(user);

      var systems = claims.Where(c => c.Type == ClaimTypes.System);

      return [.. systems];
   }

   public async Task<Result> AddSystemClaimAsync(string userId, string system)
   {
      var user = await GetUserById(userId);

      var systemClaim = new Claim(ClaimTypes.System, system);

      var identityResult = await UserManager.AddClaimAsync(user, systemClaim);

      return identityResult.ToApplicationResult();
   }
   public async Task<Result> RemoveAllSystemClaimAsync(string userId)
   {
      var user = await GetUserById(userId);

      foreach (var systemClaim in await GetSystemClaimsAsync(userId))
      {
         await UserManager.RemoveClaimAsync(user, systemClaim);
      }

      return Result.Success();
   }

   public async Task<Result> RemoveSystemClaimAsync(string userId, string system)
   {
      var user = await GetUserById(userId);

      var systemClaim = new Claim(ClaimTypes.System, system);

      var identityResult = await UserManager.RemoveClaimAsync(user, systemClaim);

      return identityResult.ToApplicationResult();
   }

   public async Task<bool> HasSystemClaimAsync(string userId, string system)
   {
      var user = await GetUserById(userId);

      var principal = await userClaimsPrincipalFactory.CreateAsync(user);

      var hasClaim = principal.HasClaim(ClaimTypes.System, system);

      return hasClaim;
   }
}
