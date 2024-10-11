using System.Security.Claims;
using invensys.iserve.Application.Common.Models;

namespace invensys.iserve.Application.Common.Interfaces;
public interface IIdentityRoleClaimService
{
   Task<Result> AddRoleClaimAsync(string userId, string role);
   Task<List<Claim>> GetRoleClaimsAsync(string userId);
   Task<bool> HasRoleClaimAsync(string userId, string claim);
   Task<Result> RemoveAllRoleClaimAsync(string userId);
   Task<Result> RemoveRoleClaimAsync(string userId, string role);
}
