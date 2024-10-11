using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Application.Common.Models;
using invensys.iserve.Application.Common.Providers;
using invensys.iserve.Application.Common.Security;
using invensys.iserve.Application.Identity.Claims;
using invensys.iserve.Domain.Constants;
using invensys.iserve.Domain.Entities;
using invensys.iserve.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static Duende.IdentityServer.Models.IdentityResources;

namespace invensys.iserve.Application.Identity.Users.Commands;

[Authorize(Policy = Policies.IserveAdmin)]
public record UpdateIdentityUserCommand : IRequest<Result>
{
   public Guid UserId { get; init; }

   public required string Email { get; init; }
   public List<string> RoleClaims { get; set; } = [];
   public List<string> SystemClaims { get; set; } = [];
}

public class UpdateIdentityUserCommandValidator : AbstractValidator<UpdateIdentityUserCommand>
{
   public UpdateIdentityUserCommandValidator()
   {

      RuleFor(req => req.Email)
         .EmailAddress()
         .MaximumLength(256)
         .NotEmpty()
         .NotNull();
   }
}

public class UpdateIdentityUserCommandHandler(IIdentityUserService identityService,
   IIdentityRoleClaimService identityRoleClaimService,
   IIdentitySystemClaimService identitySystemClaimService,
   IEmailService emailService,
   IConfiguration configuration,
   ILogger<UpdateIdentityUserCommandHandler> logger) : IRequestHandler<UpdateIdentityUserCommand, Result>
{

   public async Task<Result> Handle(UpdateIdentityUserCommand request, CancellationToken cancellationToken)
   {
      var angularSettings = configuration.GetSection(nameof(AngularSettings)).Get<AngularSettings>();

      Guard.Against.Null(angularSettings, nameof(angularSettings));

      if (!await identityService.ExistsAsync(request.Email))
      {
         var result = await identityService.UpdateUserAsync(request.UserId.ToString(), request.Email);

         logger.LogInformation("invensys.iserve user updated {Username}", request.Email);

         var token = await identityService.GeneratePasswordResetTokenAsync(request.UserId.ToString());

         var generatedPassword = PasswordGenerator.GeneratePassword();

         var resultPass = await identityService.ResetPasswordAsync(request.UserId.ToString(), token, generatedPassword);

         await emailService.SendEmailAsync(request.Email!, "iServe user registration", EmailTemplate(generatedPassword, angularSettings.SpaClientUrl));
      }

      await identityRoleClaimService.RemoveAllRoleClaimAsync(request.UserId.ToString());

      if (request.RoleClaims != null)
      {
         foreach (var role in request.RoleClaims)
         {
            await identityRoleClaimService.AddRoleClaimAsync(request.UserId.ToString(), role);
         }
      }

      await identitySystemClaimService.RemoveAllSystemClaimAsync(request.UserId.ToString());

      if (request.SystemClaims != null)
      {
         foreach (var system in request.SystemClaims)
         {
            await identitySystemClaimService.AddSystemClaimAsync(request.UserId.ToString(), system);
         }
      }

      return Result.Success();
   }

   private static string EmailTemplate(string password, string spaClientUrl) =>
      @$"<!DOCTYPE html>
      <html>
      <body>
         <img src=""https://iserve.invensys.web.za/assets/images/invensys-logo.png""  width=""144"" height=""160"" alt=""invensys-logo"">
         <p>Your user profile has been registered successfully.</p>  
         <p>Your default password is <strong>{password}</strong></p>  
         <p>Please use the following link to reset the default password. <a href=""{spaClientUrl}/auth/change-password"">Change Password.</a></p>  
      </body>
      </html>";
}
