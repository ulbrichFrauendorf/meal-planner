using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

namespace invensys.iserve.Infrastructure.Profile;
public class ClaimsProfileService() : IProfileService
{
   public Task GetProfileDataAsync(ProfileDataRequestContext context)
   {
      var claims = context.Subject.Claims.ToList();

      if (context.RequestedClaimTypes.Contains(ClaimTypes.System))
      {
         var systemClaims = claims.Where(c => c.Type == ClaimTypes.System);
         if (systemClaims != null)
         {
            context.IssuedClaims.AddRange(systemClaims);
         }
      }

      if (context.RequestedClaimTypes.Contains(ClaimTypes.Role))
      {
         var roleClaims = claims.Where(c => c.Type == ClaimTypes.Role);
         if (roleClaims != null)
         {
            context.IssuedClaims.AddRange(roleClaims);
         }
      }

      return Task.CompletedTask;
   }

   public Task IsActiveAsync(IsActiveContext context)
   {
      context.IsActive = true;
      return Task.CompletedTask;
   }
}
