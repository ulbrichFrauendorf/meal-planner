using System.Security.Claims;
using invensys.iserve.Application.Common.Interfaces;

namespace invensys.iserve.Application.Identity.Claims.Queries;

public record GetUserClaimsQuery : IRequest<List<Claim>>
{
   public required Guid UserId { get; init; }
}

public class GetUserClaimsQueryValidator : AbstractValidator<GetUserClaimsQuery>
{
   public GetUserClaimsQueryValidator() { }
}

public class GetUserClaimsQueryHandler(IIdentityRoleClaimService identityRoleClaimService, IIdentitySystemClaimService identitySystemClaimService)
   : IRequestHandler<GetUserClaimsQuery, List<Claim>>
{
   public async Task<List<Claim>> Handle(GetUserClaimsQuery request, CancellationToken cancellationToken)
   {
      var claims = new List<Claim>();

      claims.AddRange(await identityRoleClaimService.GetRoleClaimsAsync(request.UserId.ToString()));

      claims.AddRange(await identitySystemClaimService.GetSystemClaimsAsync(request.UserId.ToString()));

      return claims;
   }
}
