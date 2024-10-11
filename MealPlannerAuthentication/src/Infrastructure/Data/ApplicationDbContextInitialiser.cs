using System.Security.Claims;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using invensys.iserve.Domain.Entities;
using invensys.iserve.Infrastructure.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace invensys.iserve.Infrastructure.Data;

public static class InitialiserExtensions
{
   public static async Task InitialiseDatabaseAsync(this WebApplication app)
   {
      using var scope = app.Services.CreateScope();

      var applicationInitialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
      var configurationInitialiser = scope.ServiceProvider.GetRequiredService<ConfigurationDbContextInitialiser>();

      await applicationInitialiser.InitialiseAsync();
      await applicationInitialiser.SeedAsync();

      await configurationInitialiser.InitialiseAsync();
      await configurationInitialiser.SeedAsync();
   }
}

public class ApplicationDbContextInitialiser(
   ILogger<ApplicationDbContextInitialiser> logger,
   ApplicationDbContext context,
   UserManager<ApplicationUser> userManager,
   IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
{
   public async Task InitialiseAsync()
   {
      try
      {
         await context.Database.MigrateAsync();
      }
      catch (Exception ex)
      {
         logger.LogError(ex, "An error occurred while initialising the database.");
         throw;
      }
   }

   public async Task SeedAsync()
   {
      try
      {
         await TrySeedAsync();
      }
      catch (Exception ex)
      {
         logger.LogError(ex, "An error occurred while seeding the database.");
         throw;
      }
   }

   public async Task TrySeedAsync()
   {
      // Default users

      var adminEmail = "admin@mealplanner.web.za";
      var userEmail = "user@mealplanner.web.za";

      var administrator = new ApplicationUser
      {
         Id = Guid.NewGuid().ToString(),
         UserName = adminEmail,
         Email = adminEmail
      };

      if (userManager.Users.All(u => u.UserName != administrator.UserName))
      {
         await userManager.CreateAsync(administrator, "P@ssw0rd1");
      }
      else
      {
         administrator = await userManager.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);
      }

      await AddClaim(administrator, "mealplanner.access", ClaimTypes.System);
      await AddClaim(administrator, "mealplanner.user", ClaimTypes.Role);
      await AddClaim(administrator, "mealplanner.administrator", ClaimTypes.Role);

      var user = new ApplicationUser
      {
         Id = Guid.NewGuid().ToString(),
         UserName = userEmail,
         Email = userEmail
      };

      if (userManager.Users.All(u => u.UserName != user.UserName))
      {
         await userManager.CreateAsync(user, "P@ssw0rd1");
      }
      else
      {
         user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
      }

      await AddClaim(user, "mealplanner.access", ClaimTypes.System);
      await AddClaim(user, "mealplanner.user", ClaimTypes.Role);
   }

   private async Task AddClaim(ApplicationUser? administrator, string systemClaimStr, string claimType)
   {
      var principal = await userClaimsPrincipalFactory.CreateAsync(administrator!);

      if (!principal.HasClaim(claimType, systemClaimStr))
      {
         var systemClaim = new Claim(claimType, systemClaimStr);

         await userManager.AddClaimAsync(administrator!, systemClaim);
      }
   }
}

public class ConfigurationDbContextInitialiser(ILogger<ConfigurationDbContextInitialiser> logger,
   ConfigurationDbContext context, IConfiguration configuration)
{
   public async Task InitialiseAsync()
   {
      try
      {
         await context.Database.MigrateAsync();
      }
      catch (Exception ex)
      {
         logger.LogError(ex, "An error occurred while initialising the database.");
         throw;
      }
   }

   public async Task SeedAsync()
   {
      try
      {
         await TrySeedAsync();
      }
      catch (Exception ex)
      {
         logger.LogError(ex, "An error occurred while seeding the database.");
         throw;
      }
   }

   public async Task TrySeedAsync()
   {
      var clientsConfig = configuration.GetSection(nameof(ClientsConfig)).Get<ClientsConfig>();
      
      Guard.Against.Null(clientsConfig, nameof(ClientsConfig));

      if (!context.IdentityResources.Any())
      {
         foreach (var resource in SharedConfig.IdentityResources.ToList())
         {
            context.IdentityResources.Add(resource.ToEntity());
         }

         await context.SaveChangesAsync();
      }

      //Shared
      await CreateApiScopes(SharedConfig.ApiScopes);

      //IServeClient
      var secret = clientsConfig.IserveClients
         .First(s => s.ClientId == "a683e4dc-5254-439c-aaf7-f6d9c4530873")
         .ClientSecret!;
      await CreateApiClient(IserveConfig.Client(secret));
      await CreateApiResources(IserveConfig.ApiResources);
      await CreateApiScopes(IserveConfig.ApiScopes);

      secret = clientsConfig.IserveClients
        .First(s => s.ClientId == "351c102b-585e-493e-b457-b286f02e63ea")
        .ClientSecret!;
      await CreateApiClient(MealPlannerConfig.Client(secret));
      await CreateApiResources(MealPlannerConfig.ApiResources);
      await CreateApiScopes(MealPlannerConfig.ApiScopes);
   }

   private async Task CreateApiClient(Duende.IdentityServer.Models.Client client)
   {
      var clientEntity = context.Clients
         .Include(c => c.RedirectUris)
         .Include(c => c.PostLogoutRedirectUris)
         .Include(c => c.AllowedScopes)
         .FirstOrDefault(s => s.ClientId == client.ClientId);

      if (clientEntity == null)
      {
         context.Clients.Add(client.ToEntity());
      }
      else
      {
         clientEntity.ClientName = client.ClientName;
         clientEntity.AccessTokenLifetime = client.AccessTokenLifetime;
         clientEntity.IdentityTokenLifetime = client.IdentityTokenLifetime;


         // Remove existing related data
         clientEntity.RedirectUris.Clear();
         clientEntity.PostLogoutRedirectUris.Clear();
         clientEntity.AllowedScopes.Clear();
         clientEntity.ClientSecrets?.Clear();

         clientEntity.RedirectUris = client.RedirectUris.Select(uri => new ClientRedirectUri { RedirectUri = uri }).ToList();
         clientEntity.PostLogoutRedirectUris = client.PostLogoutRedirectUris.Select(uri => new ClientPostLogoutRedirectUri { PostLogoutRedirectUri = uri }).ToList();
         clientEntity.AllowedScopes = client.AllowedScopes.Select(scope => new ClientScope { Scope = scope }).ToList();
         if(client.ClientSecrets.Any())
         clientEntity.ClientSecrets = client.ClientSecrets
            .Select(secret => new ClientSecret
            {
               Value = secret.Value,
               Type = secret.Type
            })
            .ToList();

         context.Clients.Update(clientEntity);
      }

      await context.SaveChangesAsync();
   }

   private async Task CreateApiResources(List<Duende.IdentityServer.Models.ApiResource> apiResources)
   {
      foreach (var apiResource in apiResources)
      {
         var apiResourceEntity = context.ApiResources
            .Include(ar => ar.Scopes)
            .Include(ar => ar.UserClaims)
            .FirstOrDefault(s => s.Name == apiResource.Name);

         if (apiResourceEntity == null)
         {
            context.ApiResources.Add(apiResource.ToEntity());
         }
         else
         {
            apiResourceEntity.DisplayName = apiResource.DisplayName;

            apiResourceEntity.Scopes.Clear();
            apiResourceEntity.UserClaims.Clear();

            apiResourceEntity.Scopes = apiResource.Scopes.Select(scope => new ApiResourceScope { Scope = scope }).ToList();
            apiResourceEntity.UserClaims = apiResource.UserClaims.Select(claim => new ApiResourceClaim { Type = claim }).ToList();

            context.ApiResources.Update(apiResourceEntity);
         }
      }

      await context.SaveChangesAsync();
   }

   private async Task CreateApiScopes(List<Duende.IdentityServer.Models.ApiScope> apiScopes)
   {
      foreach (var apiScope in apiScopes)
      {
         var apiScopeEntity = context.ApiScopes
            .Include(s => s.UserClaims)
            .FirstOrDefault(s => s.Name == apiScope.Name);

         if (apiScopeEntity == null)
         {
            context.ApiScopes.Add(apiScope.ToEntity());
         }
         else
         {
            apiScopeEntity.DisplayName = apiScope.DisplayName;

            apiScopeEntity.UserClaims.Clear();

            apiScopeEntity.UserClaims = apiScope.UserClaims.Select(claim => new ApiScopeClaim { Type = claim }).ToList();

            context.ApiScopes.Update(apiScopeEntity);
         }
      }

      await context.SaveChangesAsync();
   }
}

//Todo Update Mappers.
