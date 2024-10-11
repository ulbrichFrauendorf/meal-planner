using System.Security.Claims;
using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Application.Common.Models;
using invensys.iserve.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace invensys.iserve.Infrastructure.Identity;

public class IdentityRoleClaimService(
   UserManager<ApplicationUser> userManager,
   IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
   : IdentityService(userManager), IIdentityRoleClaimService
{
   public async Task<List<Claim>> GetRoleClaimsAsync(string userId)
   {
      var user = await GetUserById(userId);

      var claims = await UserManager.GetClaimsAsync(user);

      var roles = claims.Where(c => c.Type == ClaimTypes.Role);

      return [.. roles];
   }

   public async Task<Result> AddRoleClaimAsync(string userId, string role)
   {
      var user = await GetUserById(userId);

      var roleClaim = new Claim(ClaimTypes.Role, role);

      var identityResult = await UserManager.AddClaimAsync(user, roleClaim);

      return identityResult.ToApplicationResult();
   }

   public async Task<Result> RemoveAllRoleClaimAsync(string userId)
   {
      var user = await GetUserById(userId);

      foreach (var roleClaim in await GetRoleClaimsAsync(userId))
      {
         await UserManager.RemoveClaimAsync(user, roleClaim);
      }

      return Result.Success();
   }

   public async Task<Result> RemoveRoleClaimAsync(string userId, string role)
   {
      var user = await GetUserById(userId);

      var roleClaim = new Claim(ClaimTypes.Role, role);

      var identityResult = await UserManager.RemoveClaimAsync(user, roleClaim);

      return identityResult.ToApplicationResult();
   }

   public async Task<bool> HasRoleClaimAsync(string userId, string claim)
   {
      var user = await GetUserById(userId);

      var principal = await userClaimsPrincipalFactory.CreateAsync(user);

      var hasClaim = principal.HasClaim(ClaimTypes.Role, claim);

      return hasClaim;
   }
}
