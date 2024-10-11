using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Domain.Constants;

namespace invensys.iserve.Application.Identity.Claims.Queries;

public record HasClaimQuery : IRequest<bool>
{
   public required string ClaimName { get; init; }
}

public class HasClaimQueryValidator : AbstractValidator<HasClaimQuery>
{
   public HasClaimQueryValidator() { }
}

public class HasClaimQueryHandler(IIdentityRoleClaimService identityRoleClaimService,
   IIdentitySystemClaimService identitySystemClaimService,
   IUser user)
   : IRequestHandler<HasClaimQuery, bool>
{
   public async Task<bool> Handle(HasClaimQuery request, CancellationToken cancellationToken)
   {
      var isInRole = await identityRoleClaimService.HasRoleClaimAsync(user.Id!, request.ClaimName);

      var hasSystemClaim = await identitySystemClaimService.HasSystemClaimAsync(user.Id!, SystemClaim.IServeAccess);

      return isInRole && hasSystemClaim;
   }
}
