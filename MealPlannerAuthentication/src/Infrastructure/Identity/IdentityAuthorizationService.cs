using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace invensys.iserve.Infrastructure.Identity;

public class IdentityAuthorizationService(
    UserManager<ApplicationUser> userManager,
    IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
    IAuthorizationService authorizationService) : IdentityService(userManager), IIdentityAuthorizationService
{
   public async Task<bool> AuthorizeAsync(string userId, string policyName)
   {
      var user = await GetUserById(userId);

      var principal = await userClaimsPrincipalFactory.CreateAsync(user);

      var result = await authorizationService.AuthorizeAsync(principal, policyName);

      return result.Succeeded;
   }
}
