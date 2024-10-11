using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Application.Common.Security;
using invensys.iserve.Domain.Constants;

namespace invensys.iserve.Application.Identity.Users.Commands;

[Authorize(Policy = Policies.IserveAdmin)]
public record DeleteIdentityUserCommand : IRequest
{
   public Guid UserId { get; init; }
}

public class DeleteIdentityUserCommandValidator : AbstractValidator<DeleteIdentityUserCommand>
{
}

public class DeleteIdentityUserCommandHandler(IIdentityUserService identityService)
   : IRequestHandler<DeleteIdentityUserCommand>
{

   public async Task Handle(DeleteIdentityUserCommand request, CancellationToken cancellationToken)
   {
      await identityService.DeleteUserAsync(request.UserId.ToString());
   }
}
