namespace invensys.iserve.Application.Common.Interfaces;

public interface IIdentityAuthorizationService
{
   Task<bool> AuthorizeAsync(string userId, string policyName);
}
