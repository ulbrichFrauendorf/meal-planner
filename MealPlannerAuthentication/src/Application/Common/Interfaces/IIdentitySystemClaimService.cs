using System.Security.Claims;
using invensys.iserve.Application.Common.Models;

namespace invensys.iserve.Application.Common.Interfaces;
public interface IIdentitySystemClaimService
{
   Task<Result> AddSystemClaimAsync(string userId, string system);
   Task<List<Claim>> GetSystemClaimsAsync(string userId);
   Task<bool> HasSystemClaimAsync(string userId, string system);
   Task<Result> RemoveAllSystemClaimAsync(string userId);
   Task<Result> RemoveSystemClaimAsync(string userId, string system);

}
