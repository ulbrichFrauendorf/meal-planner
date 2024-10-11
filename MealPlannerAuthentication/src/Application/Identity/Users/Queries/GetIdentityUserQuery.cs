using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Application.Common.Security;
using invensys.iserve.Domain.Constants;

namespace invensys.iserve.Application.Identity.Users.Queries;
[Authorize(Policy = Policies.IserveAdmin)]
public record GetIdentityUserQuery : IRequest<IdentityUserVM>
{
   public Guid UserId { get; init; }
}

public class GetIdentityUserQueryValidator : AbstractValidator<GetIdentityUserQuery>
{
   public GetIdentityUserQueryValidator()
   {
   }
}

public class GetIdentityUserQueryHandler(IIdentityUserService identityService,
   IIdentityRoleClaimService identityRoleClaimService, IIdentitySystemClaimService identitySystemClaimService,
   IMapper mapper)
   : IRequestHandler<GetIdentityUserQuery, IdentityUserVM>
{
   public async Task<IdentityUserVM> Handle(GetIdentityUserQuery request, CancellationToken cancellationToken)
   {
      var userId = request.UserId.ToString();

      var user = await identityService.GetApplicationUser(userId);

      var roleClaims = await identityRoleClaimService.GetRoleClaimsAsync(userId);

      var systemClaims = await identitySystemClaimService.GetSystemClaimsAsync(userId);
      
      return new IdentityUserVM
      {
         IdentityUser = mapper.Map<IdentityUserDto>(user),
         RoleClaims = [.. roleClaims.Select(c => c.Value)],
         SystemClaims = [.. systemClaims.Select(c => c.Value)]
      };
   }
}
