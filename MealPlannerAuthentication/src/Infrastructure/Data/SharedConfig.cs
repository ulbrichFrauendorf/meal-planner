using Duende.IdentityServer.Models;

namespace invensys.iserve.Infrastructure.Data;

public static class SharedConfig
{
   public static IEnumerable<IdentityResource> IdentityResources =>
       new IdentityResource[]
       {
          new IdentityResources.OpenId(),
         new IdentityResources.Profile(),
         new IdentityResources.Email(),
       };


   public static List<ApiScope> ApiScopes =>
       new()
       {
          new ApiScope("api.manage", "Api Shared Management")
       };
}
