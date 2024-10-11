using invensys.iserve.Application.Common.Models;
using invensys.iserve.Domain.Entities;

namespace invensys.iserve.Application.Common.Interfaces;
public interface IIdentityUserService
{
   Task<Result> ChangeUserPasswordAsync(string userId, string currentPassword, string newPassword);
   Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);
   Task<Result> DeleteUserAsync(string userId);
   Task<bool> ExistsAsync(string email);
   Task<string?> ForgotPasswordAsync(string userId);
   IQueryable<ApplicationUser> GetApplicationUsers();
   Task<ApplicationUser> GetApplicationUser(string userId);
   Task<string?> GetUserNameAsync(string userId);
   Task<Result> UpdateUserAsync(string userId, string userName);
   Task<string> GeneratePasswordResetTokenAsync(string userId);
   Task<Result> ResetPasswordAsync(string userId, string token, string newPassword);
   Task<ApplicationUser> GetApplicationUserByEmail(string email);
}
