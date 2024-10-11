using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Application.Common.Providers;
using invensys.iserve.Application.Common.Security;
using invensys.iserve.Application.Identity.Claims;
using invensys.iserve.Domain.Constants;
using invensys.iserve.Domain.Entities;
using invensys.iserve.Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace invensys.iserve.Application.Identity.Users.Commands;

[Authorize(Policy = Policies.IserveAdmin)]
public record CreateIdentityUserCommand : IRequest<string>
{
   public string? Email { get; init; }
   
   public List<string>? RoleClaims { get; init; } = [];

   public List<string>? SystemClaims { get; init; } = [];

}

public class CreateIdentityUserCommandValidator : AbstractValidator<CreateIdentityUserCommand>
{
   private readonly IIdentityUserService _identityService;

   public CreateIdentityUserCommandValidator(IIdentityUserService identityService)
   {
      _identityService = identityService;

      RuleFor(req => req.Email)
         .MaximumLength(256)
         .NotEmpty()
         .NotNull()
         .MustAsync(BeUniqueEmail)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
   }

   public async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
   {
      return !await _identityService.ExistsAsync(email);
   }

}

public class CreateIdentityUserCommandHandler(IIdentityUserService identityService,
   IIdentityRoleClaimService identityRoleClaimService,
   IIdentitySystemClaimService identitySystemClaimService,
   IEmailService emailService,
   IConfiguration configuration,
   ILogger<CreateIdentityUserCommandHandler> logger) : IRequestHandler<CreateIdentityUserCommand, string?>
{

   public async Task<string?> Handle(CreateIdentityUserCommand request, CancellationToken cancellationToken)
   {
      var angularSettings = configuration.GetSection(nameof(AngularSettings)).Get<AngularSettings>();

      Guard.Against.Null(angularSettings, nameof(angularSettings));

      var generatedPassword = PasswordGenerator.GeneratePassword();
      
      var (result, userId) = await identityService.CreateUserAsync(request.Email!, generatedPassword);

      if (request.RoleClaims != null)
      {
         foreach (var role in request.RoleClaims)
         {
            await identityRoleClaimService.AddRoleClaimAsync(userId, role);
         }
      }

      if (request.SystemClaims != null)
      {
         foreach (var system in request.SystemClaims)
         {
            await identitySystemClaimService.AddSystemClaimAsync(userId, system);
         }
      }

      logger.LogInformation("invensys.iserve New user created {Username}", request.Email);

      await emailService.SendEmailAsync(request.Email!, "iServe user registration", EmailTemplate(generatedPassword, angularSettings.SpaClientUrl));

      return userId;
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
