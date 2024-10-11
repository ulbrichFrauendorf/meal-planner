using invensys.iserve.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace invensys.iserve.Infrastructure.Identity;

public abstract class IdentityService(
      UserManager<ApplicationUser> userManager)
{
   public readonly UserManager<ApplicationUser> UserManager = userManager;

   public async Task<ApplicationUser> GetUserById(string userId)
   {
      var user = await UserManager.FindByIdAsync(userId);

      Guard.Against.NotFound(userId, user);

      return user;
   }

   public async Task<ApplicationUser> GetUserByEmail(string email)
   {
      var user = await UserManager.Users.FirstOrDefaultAsync(u => u.Email == email);

      Guard.Against.NotFound(email, user);

      return user;
   }
}
