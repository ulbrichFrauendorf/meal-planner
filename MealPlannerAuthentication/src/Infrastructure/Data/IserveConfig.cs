using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Microsoft.Extensions.Configuration;

namespace invensys.iserve.Infrastructure.Data;

public static class IserveConfig
{
   private const string spaIServeDevClientUrl = "https://localhost:44448";
   private const string spaIserveClientUrl = "https://iserve.invensys.web.za";

   public static List<ApiResource> ApiResources =>
      new()
      {
         new("iserve.api", "Invensys Iserve Resource Api")
         {
            Scopes = { "iserve.users", "iserve.clients" },
            UserClaims = { ClaimTypes.Role, ClaimTypes.System }
         }
      };

   public static List<ApiScope> ApiScopes =>
      new()
      {
         new ApiScope("iserve.users", "Api Users Access - Invensys Iserve"),
         new ApiScope("iserve.clients", "Api Clients Access - Invensys Iserve")
      };

   public static Client Client(string secret) =>
      new()
      {
         ClientId = "a683e4dc-5254-439c-aaf7-f6d9c4530873",
         ClientName = "Invensys IServe Client",
         AccessTokenType = (int)AccessTokenType.Jwt,
         AccessTokenLifetime = 330,
         IdentityTokenLifetime = 30,
         AllowOfflineAccess = true,
         RequireClientSecret = false,
         AllowedGrantTypes = GrantTypes.Code,
         RequirePkce = true,
         AllowAccessTokensViaBrowser = true,
         RedirectUris =
            new List<string>
            {
               $"{spaIServeDevClientUrl}/callback",
               $"{spaIServeDevClientUrl}/silent-renew.html",
               $"{spaIserveClientUrl}/callback",
               $"{spaIserveClientUrl}/silent-renew.html"
            },
         PostLogoutRedirectUris = new List<string>
         {
            $"{spaIServeDevClientUrl}/unauthorized",
            $"{spaIServeDevClientUrl}",
            $"{spaIserveClientUrl}/unauthorized",
            $"{spaIserveClientUrl}",
         },
         AllowedCorsOrigins = new List<string> { $"{spaIServeDevClientUrl}", $"{spaIserveClientUrl}" },
         AllowedScopes = new List<string>
         {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            IdentityServerConstants.StandardScopes.Email,
            "iserve.users",
            "iserve.clients"
         }
      };
}

