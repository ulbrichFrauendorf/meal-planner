using System.Security.Claims;
using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Application.Common.Models;
using invensys.iserve.Application.Common.Security;
using invensys.iserve.Domain.Constants;
using invensys.iserve.Domain.ValueObjects;

namespace invensys.iserve.Application.Identity.Claims.Commands;

[Authorize(Policy = Policies.IserveAdmin)]
public record RemoveClaimCommand : IRequest<Result>
{
   public Guid UserId { get; set; }

   public required Claim Claim { get; set; }
}

public class RemoveClaimCommandValidator : AbstractValidator<RemoveClaimCommand>
{
}

public class RemoveClaimCommandHandler(IIdentityRoleClaimService identityRoleClaimService, IIdentitySystemClaimService identitySystemClaimService) : IRequestHandler<RemoveClaimCommand, Result>
{
   public async Task<Result> Handle(RemoveClaimCommand request, CancellationToken cancellationToken)
   {
      return request.Claim.Type switch
      {
         ClaimTypes.Role => await identityRoleClaimService.RemoveRoleClaimAsync(request.UserId.ToString(), request.Claim.Value),
         ClaimTypes.System => await identitySystemClaimService.RemoveSystemClaimAsync(request.UserId.ToString(), request.Claim.Value),
         _ => throw new ArgumentException("Claim not used by system."),
      };
   }
}
