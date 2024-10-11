using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Application.Common.Mappings;
using invensys.iserve.Application.Common.Security;
using invensys.iserve.Domain.Constants;

namespace invensys.iserve.Application.Identity.Users.Queries;
[Authorize(Policy = Policies.IserveAdmin)]
public record GetIdentityUsersQuery : IRequest<List<IdentityUserVM>>
{
}

public class GetIdentityUsersQueryValidator : AbstractValidator<GetIdentityUsersQuery>
{
   public GetIdentityUsersQueryValidator()
   {
   }
}

public class GetIdentityUsersQueryHandler(IIdentityUserService identityService,
   IIdentityRoleClaimService identityRoleClaimService, IIdentitySystemClaimService identitySystemClaimService,
   IMapper mapper) : IRequestHandler<GetIdentityUsersQuery, List<IdentityUserVM>>
{
   public async Task<List<IdentityUserVM>> Handle(GetIdentityUsersQuery request, CancellationToken cancellationToken)
   {
      var users = identityService.GetApplicationUsers();

      var projectedUsers = await users.ProjectToListAsync<IdentityUserDto>(mapper.ConfigurationProvider);

      var identityUsers = new List<IdentityUserVM>();

      foreach(var user in projectedUsers)
      {

         var roleClaims = await identityRoleClaimService.GetRoleClaimsAsync(user.Id!);

         var systemClaims = await identitySystemClaimService.GetSystemClaimsAsync(user.Id!);

         identityUsers.Add(new IdentityUserVM
         {
            IdentityUser = user,
            RoleClaims = [.. roleClaims.Select(c => c.Value)],
            SystemClaims = [.. systemClaims.Select(c => c.Value)]
         });
      }

      return identityUsers;
   }
}
