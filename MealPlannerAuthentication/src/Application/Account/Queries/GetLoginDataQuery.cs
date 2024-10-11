using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using invensys.iserve.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace invensys.iserve.Application.Account.Queries;

public record GetLoginDataQuery : IRequest<LoginDto>
{
   public string? RedirectUrl { get; set; }
}

public static class LoginOptions
{
   public static readonly bool AllowLocalLogin = true;
   public static readonly bool AllowRememberLogin = true;
   public static readonly TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
   public static readonly string InvalidCredentialsErrorMessage = "Invalid username or password";
}

public class LoginDto
{
   public bool AllowRememberLogin { get; set; } = true;

   public bool EnableLocalLogin { get; set; } = true;

   public List<ExternalProvider> ExternalProviders { get; set; } = [];

   public List<ExternalProvider> VisibleExternalProviders =>
      ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName)).ToList();

   public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;

   public string? ExternalLoginScheme =>
      IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

   public class ExternalProvider
   {
      public ExternalProvider(string authenticationScheme, string? displayName = null)
      {
         AuthenticationScheme = authenticationScheme;
         DisplayName = displayName;
      }

      public string? DisplayName { get; set; }
      public string AuthenticationScheme { get; set; }
   }
}

public class GetLoginDataQueryValidator : AbstractValidator<GetLoginDataQuery>
{
   public GetLoginDataQueryValidator()
   {
   }
}

public class GetLoginDataQueryHandler(
   IIdentityServerInteractionService interaction,
   IAuthenticationSchemeProvider schemeProvider,
   IIdentityProviderStore identityProviderStore,
   ILocalServerHttpContextAccessor localServerHttpContextAccessor
   ) : IRequestHandler<GetLoginDataQuery, LoginDto>
{
   public async Task<LoginDto> Handle(GetLoginDataQuery request, CancellationToken cancellationToken)
   {
      return await BuildModelAsync(request.RedirectUrl);
   }

   private async Task<LoginDto> BuildModelAsync(string? returnUrl)
   {
      var loginData = new LoginDto();

      var context = await interaction.GetAuthorizationContextAsync(
         localServerHttpContextAccessor
            .TrimCurrentSiteReturnUrl(returnUrl)
          );

      if (context?.IdP != null && await schemeProvider.GetSchemeAsync(context.IdP) != null)
      {
         var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;

         loginData.EnableLocalLogin = local;

         if (!local)
         {
            loginData.ExternalProviders = [new LoginDto.ExternalProvider(authenticationScheme: context.IdP)];
         }

         return loginData;
      }

      var schemes = await schemeProvider.GetAllSchemesAsync();

      var providers = schemes
         .Where(x => x.DisplayName != null)
         .Select(x => new LoginDto.ExternalProvider
         (
            authenticationScheme: x.Name,
            displayName: x.DisplayName ?? x.Name
         )).ToList();

      var dynamicSchemes = (await identityProviderStore.GetAllSchemeNamesAsync())
         .Where(x => x.Enabled)
         .Select(x => new LoginDto.ExternalProvider
         (
            authenticationScheme: x.Scheme,
            displayName: x.DisplayName ?? x.Scheme
         ));

      providers.AddRange(dynamicSchemes);

      var allowLocal = true;

      var client = context?.Client;

      if (client != null)
      {
         allowLocal = client.EnableLocalLogin;
         if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Count != 0)
         {
            providers = providers
               .Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
         }
      }

      loginData.AllowRememberLogin = LoginOptions.AllowRememberLogin;

      loginData.EnableLocalLogin = allowLocal && LoginOptions.AllowLocalLogin;

      loginData.ExternalProviders = providers;

      return loginData;
   }
}
