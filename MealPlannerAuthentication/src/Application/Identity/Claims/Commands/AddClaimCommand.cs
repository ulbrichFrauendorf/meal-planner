using System.Security.Claims;
using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Application.Common.Models;
using invensys.iserve.Application.Common.Security;
using invensys.iserve.Domain.Constants;
using invensys.iserve.Domain.ValueObjects;

namespace invensys.iserve.Application.Identity.Claims.Commands;

[Authorize(Policy = Policies.IserveAdmin)]
public record AddClaimCommand : IRequest<Result>
{
   public Guid UserId { get; set; }

   public required Claim Claim { get; set; }
}

public class AddClaimCommandValidator : AbstractValidator<AddClaimCommand>
{
}

public class AddClaimCommandHandler(IIdentityRoleClaimService identityRoleClaimService, IIdentitySystemClaimService identitySystemClaimService) : IRequestHandler<AddClaimCommand, Result>
{
   public async Task<Result> Handle(AddClaimCommand request, CancellationToken cancellationToken)
   {
      return request.Claim.Type switch
      {
         ClaimTypes.Role => await identityRoleClaimService.AddRoleClaimAsync(request.UserId.ToString(), request.Claim.Value),
         ClaimTypes.System => await identitySystemClaimService.AddSystemClaimAsync(request.UserId.ToString(), request.Claim.Value),
         _ => throw new ArgumentException("Claim not used by system."),
      };
   }
}
