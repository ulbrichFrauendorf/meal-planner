using invensys.iserve.Application.Common.Exceptions;
using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Application.Common.Models;
using invensys.iserve.Domain.ValueObjects;

namespace invensys.iserve.Application.Account.Commands;

public record ChangePasswordCommand() : IRequest<Result>
{
   public required string CurrentPassword { get; set; }
   public required string NewPassword { get; set; }
   public required string ConfirmNewPassword { get; set; }
};

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{

   public ChangePasswordCommandValidator()
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

public class ChangePasswordCommandHandler(
   IUser user,
   IIdentityUserService identityService) : IRequestHandler<ChangePasswordCommand, Result>
{
   public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
   {
      var userId = user.Id;
      Guard.Against.NullOrEmpty(userId, nameof(userId));

      var result = await identityService.ChangeUserPasswordAsync(userId, request.CurrentPassword, request.NewPassword);

      return !result.Succeeded ? throw new FrontEndApiException(string.Join(", ", result.Errors)) : result;
   }
}
