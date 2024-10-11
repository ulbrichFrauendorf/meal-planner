using System.Diagnostics;
using MealPlanner.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace MealPlanner.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse>(
	ILogger<TRequest> logger,
	IUser user,
	IIdentityService identityService
) : IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull
{
	private readonly Stopwatch _timer = new Stopwatch();
	private readonly ILogger<TRequest> _logger = logger;
	private readonly IUser _user = user;
	private readonly IIdentityService _identityService = identityService;

	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken
	)
	{
		_timer.Start();

		var response = await next();

		_timer.Stop();

		var elapsedMilliseconds = _timer.ElapsedMilliseconds;

		if (elapsedMilliseconds > 500)
		{
			var requestName = typeof(TRequest).Name;
			var userId = _user.Id ?? string.Empty;
			var userName = _identityService.GetUserName();

			_logger.LogWarning(
				"MealPlanner Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
				requestName,
				elapsedMilliseconds,
				userId,
				userName,
				request
			);
		}

		return response;
	}
}
