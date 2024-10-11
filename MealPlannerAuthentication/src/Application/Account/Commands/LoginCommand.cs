using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Telemetry = invensys.iserve.Application.Common.Extensions.Telemetry;

namespace invensys.iserve.Application.Account.Commands;

public enum LoginAction
{
   Login,
   Cancel
}

public class LoginResponse
{
   public bool IsSuccess { get; set; }
   public string? RedirectUrl { get; set; }
   public string? ErrorMessage { get; set; }
}

public record LoginCommand : IRequest<LoginResponse>
{
   public required string Username { get; init; }
   public required string Password { get; init; }
   public bool RememberLogin { get; init; }
   public string? ReturnUrl { get; init; }
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
   public LoginCommandValidator()
   {
      RuleFor(s => s.ReturnUrl)
         .NotEmpty()
         .NotNull();

      RuleFor(s => s.Username)
         .NotEmpty()
         .NotNull();

      RuleFor(s => s.Password)
         .NotEmpty()
         .NotNull();
   }
}

public class LoginCommandHandler(IIdentityServerInteractionService interaction,
   IEventService events,
   UserManager<ApplicationUser> userManager,
   IClientStore clientStore,
   SignInManager<ApplicationUser> signInManager,
   ILocalServerHttpContextAccessor localServerHttpContextAccessor
   )
   : IRequestHandler<LoginCommand, LoginResponse>
{
   public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
   {
      var url = request.ReturnUrl != null ? Uri.UnescapeDataString(request.ReturnUrl) : null;

      url = localServerHttpContextAccessor.TrimCurrentSiteReturnUrl(url);

      var context = await interaction.GetAuthorizationContextAsync(url);

      if (context == null)
      {
         return new LoginResponse { IsSuccess = false, ErrorMessage = "Invalid login request." };
      }

      var result = await signInManager.PasswordSignInAsync(request.Username, request.Password, false, lockoutOnFailure: true);

      if (result.Succeeded)
      {
         var user = await userManager.FindByNameAsync(request.Username);

         Guard.Against.Null(user, nameof(user));

         await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context.Client.ClientId));

         Telemetry.Metrics.UserLogin(context.Client.ClientId, IdentityServerConstants.LocalIdentityProvider);

         var redirectUrl = context.RedirectUri;

         var client = await clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);

         if (client != null && client.RedirectUris.Contains(redirectUrl))
         {
            return new LoginResponse { IsSuccess = true, RedirectUrl = request.ReturnUrl };
         }
      }

      return new LoginResponse { IsSuccess = false, ErrorMessage = "Invalid credentials." };
   }


}
