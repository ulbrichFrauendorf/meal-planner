using MealPlanner.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace MealPlanner.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest>(
	ILogger<TRequest> logger,
	IUser user,
	IIdentityService identityService
) : IRequestPreProcessor<TRequest>
	where TRequest : notnull
{
	private readonly ILogger _logger = logger;
	private readonly IUser _user = user;
	private readonly IIdentityService _identityService = identityService;

	public async Task Process(TRequest request, CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;
		var userId = _user.Id;
		var userName = _identityService.GetUserName();

		_logger.LogInformation(
			"MealPlanner Request: {Name} {@UserId} {@UserName} {@Request}",
			requestName,
			userId,
			userName,
			request
		);

		await Task.CompletedTask;
	}
}
