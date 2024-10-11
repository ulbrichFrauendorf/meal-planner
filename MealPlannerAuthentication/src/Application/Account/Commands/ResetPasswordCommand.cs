using invensys.iserve.Application.Common.Exceptions;
using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Application.Common.Models;
using invensys.iserve.Domain.ValueObjects;

namespace invensys.iserve.Application.Account.Commands;

public record ResetPasswordCommand() : IRequest<Result>
{
   public required string ResetToken { get; set; }
   public required string Email { get; set; }
   public required string NewPassword { get; set; }
   public required string ConfirmNewPassword { get; set; }
};

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
   public ResetPasswordCommandValidator()
   {
      RuleFor(req => req.NewPassword)
         .NotEmpty()
         .NotNull()
         .Length(8, 15)
         .Matches(Pattern.Password);

      RuleFor(req =>
            new BeSameAsConfirmRecord { ConfirmNewPassword = req.ConfirmNewPassword, NewPassword = req.NewPassword })
         .Must(BeSameAsConfirm)
         .WithMessage("Passwords do not match.")
         .WithErrorCode("Authorization");
   }

   private record BeSameAsConfirmRecord()
   {
      public required string NewPassword { get; set; }
      public required string ConfirmNewPassword { get; set; }
   }

   private bool BeSameAsConfirm(BeSameAsConfirmRecord record)
   {
      return record.ConfirmNewPassword == record.NewPassword;
   }
}

public class ResetPasswordCommandHandler(
   IIdentityUserService identityService) : IRequestHandler<ResetPasswordCommand, Result>
{
   public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
   {
      var user = await identityService.GetApplicationUserByEmail(request.Email);

      Guard.Against.Null(user, nameof(user));

      var result = await identityService.ResetPasswordAsync(user.Id, request.ResetToken, request.NewPassword);

      return !result.Succeeded ? throw new FrontEndApiException(string.Join(", ", result.Errors)) : result;
   }
}
