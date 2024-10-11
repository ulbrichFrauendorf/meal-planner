using MealPlanner.Application.Common.Interfaces;

namespace MealPlanner.Application.Users.Queries;

public record HasClaimQuery : IRequest<bool>
{
	public required string ClaimName { get; init; }
}

public class HasClaimQueryValidator : AbstractValidator<HasClaimQuery>
{
	public HasClaimQueryValidator() { }
}

public class HasClaimQueryHandler(IIdentityService identityService)
	: IRequestHandler<HasClaimQuery, bool>
{
	public async Task<bool> Handle(HasClaimQuery request, CancellationToken cancellationToken)
	{
		
		await Task.CompletedTask;

		var isInRole = identityService.IsInRole(request.ClaimName);

		var hasSystemClaim = identityService.HasSystemClaim("mealplanner.access");

		return isInRole && hasSystemClaim;
	}
}
