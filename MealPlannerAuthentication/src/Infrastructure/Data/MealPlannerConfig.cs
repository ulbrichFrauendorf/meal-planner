using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace invensys.iserve.Infrastructure.Data;
internal class MealPlannerConfig
{
   private const string MealplannerDevClientUrl = "https://localhost:44447";

   public static List<ApiResource> ApiResources =>
      new()
      {
         new("mealplanner.api", "Mealplanner Resource Api")
         {
            Scopes = { "mealplanner.api.scope" },
            UserClaims = { ClaimTypes.Role, ClaimTypes.System }
         }
      };

   public static List<ApiScope> ApiScopes =>
      new()
      {
         new ApiScope("mealplanner.api.scope", "Api Access Scope"),
      };

   public static Client Client(string secret) =>
      new()
      {
         ClientId = "351c102b-585e-493e-b457-b286f02e63ea",
         ClientName = "Meal Planner Client",
         RequireConsent = false,
         AllowedGrantTypes = GrantTypes.Code,
         AccessTokenType = AccessTokenType.Jwt,
         AccessTokenLifetime = 330,
         IdentityTokenLifetime = 330,
         AllowOfflineAccess = true,
         RequireClientSecret = false,
         RequirePkce = true,
         AllowAccessTokensViaBrowser = true,
         RedirectUris = new List<string>
         {
            $"{MealplannerDevClientUrl}/callback",
            $"{MealplannerDevClientUrl}",
         },
         PostLogoutRedirectUris = new List<string>
         {
            $"{MealplannerDevClientUrl}/unauthorized",
            $"{MealplannerDevClientUrl}",
         },
         AllowedCorsOrigins = new List<string> { $"{MealplannerDevClientUrl}" },
         AllowedScopes = new List<string>
         {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            IdentityServerConstants.StandardScopes.Email,
            IdentityServerConstants.StandardScopes.OfflineAccess,
            "mealplanner.api.scope"
         }
      };
}
