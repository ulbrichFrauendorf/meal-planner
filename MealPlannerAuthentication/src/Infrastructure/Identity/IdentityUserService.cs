using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Application.Common.Models;
using invensys.iserve.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Duende.IdentityServer.Models.IdentityResources;

namespace invensys.iserve.Infrastructure.Identity;

public class IdentityUserService(
    UserManager<ApplicationUser> userManager) : IdentityService(userManager), IIdentityUserService
{

   public async Task<string?> GetUserNameAsync(string userId)
   {
      var user = await GetUserById(userId);

      return user.UserName;
   }

   public IQueryable<ApplicationUser> GetApplicationUsers()
   {
      return UserManager.Users;
   }

   public async Task<ApplicationUser> GetApplicationUser(string userId)
   {
      return await GetUserById(userId);
   }

   public async Task<ApplicationUser> GetApplicationUserByEmail(string email)
   {
      var user = await GetUserByEmail(email);

      return await GetUserById(user.Id);
   }

   public async Task<bool> ExistsAsync(string email)
   {
      var user = await UserManager.Users.FirstOrDefaultAsync(u => u.Email == email);

      return user != null;
   }

   public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
   {
      var user = new ApplicationUser
      {
         UserName = userName,
         Email = userName,
      };

      var result = await UserManager.CreateAsync(user, password);

      return (result.ToApplicationResult(), user.Id);
   }

   public async Task<Result> UpdateUserAsync(string userId, string userName)
   {
      var user = await GetUserById(userId);

      if (user.UserName != userName)
      {
         user.UserName = userName;
         user.Email = userName;
      }

      var updateResult = await UserManager.UpdateAsync(user);

      return updateResult.ToApplicationResult();
   }

   public async Task<Result> DeleteUserAsync(string userId)
   {
      var user = await GetUserById(userId);

      var deleteResult = await UserManager.DeleteAsync(user);

      return deleteResult.ToApplicationResult();
   }

   public async Task<Result> ChangeUserPasswordAsync(string userId, string currentPassword, string newPassword)
   {
      var user = await GetUserById(userId);

      var result = await UserManager.ChangePasswordAsync(user, currentPassword, newPassword);

      return result.ToApplicationResult();
   }

   public async Task<string> GeneratePasswordResetTokenAsync(string userId)
   {
      var user = await GetUserById(userId);
      if (user == null)
      {
         throw new Exception("User not found.");
      }

      return await UserManager.GeneratePasswordResetTokenAsync(user);
   }

   public async Task<Result> ResetPasswordAsync(string userId, string token, string newPassword)
   {
      var user = await GetUserById(userId);
      if (user == null)
      {
         throw new Exception("User not found.");
      }

      var resetResult = await UserManager.ResetPasswordAsync(user, token, newPassword);
      return resetResult.ToApplicationResult();
   }

   public Task<string?> ForgotPasswordAsync(string userId)
   {
      throw new NotImplementedException();
   }
}
