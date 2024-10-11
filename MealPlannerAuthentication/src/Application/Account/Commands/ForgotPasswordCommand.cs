using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Application.Common.Models;
using invensys.iserve.Application.Common.Providers;
using invensys.iserve.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace invensys.iserve.Application.Account.Commands;

public record ForgotPasswordCommand : IRequest<Result>
{
   public required string Email { get; init; }
}

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
   public ForgotPasswordCommandValidator()
   {

   }
}

public class ForgotPasswordCommandHandler(IIdentityUserService identityService,
   IEmailService emailService,
   IConfiguration configuration
   ) : IRequestHandler<ForgotPasswordCommand, Result>
{

   public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
   {
      var angularSettings = configuration.GetSection(nameof(AngularSettings)).Get<AngularSettings>();

      Guard.Against.Null(angularSettings, nameof(angularSettings));

      var user = await identityService.GetApplicationUserByEmail(request.Email);

      var token = await identityService.GeneratePasswordResetTokenAsync(user.Id);

      var generatedPassword = PasswordGenerator.GeneratePassword();

      var resultPass = await identityService.ResetPasswordAsync(user.Id, token, generatedPassword);

     await emailService.SendEmailAsync(request.Email!, "Your password has been reset.", EmailTemplate(generatedPassword, angularSettings.SpaClientUrl));

      return Result.Success();
   }

   private static string EmailTemplate(string password, string spaClientUrl) =>
   @$"<!DOCTYPE html>
      <html>
      <body>
         <img src=""https://iserve.invensys.web.za/assets/images/invensys-logo.png""  width=""144"" height=""160"" alt=""invensys-logo"">
         <p>Your password have been reset successfully.</p>  
         <p>Your password has been changed to: <strong>{password}</strong></p>  
         <p>Please use the following link to reset your password. <a href=""{spaClientUrl}/auth/change-password"">Change Password.</a></p>  
      </body>
      </html>";
}
