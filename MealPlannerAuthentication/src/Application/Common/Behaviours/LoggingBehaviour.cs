using invensys.iserve.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace invensys.iserve.Application.Common.Behaviours;
public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
   private readonly ILogger _logger;
   private readonly IUser _user;
   private readonly IIdentityUserService _identityService;

   public LoggingBehaviour(ILogger<TRequest> logger, IUser user, IIdentityUserService identityService)
   {
      _logger = logger;
      _user = user;
      _identityService = identityService;
   }

   public async Task Process(TRequest request, CancellationToken cancellationToken)
   {
      var requestName = typeof(TRequest).Name;
      var userId = _user.Id ?? string.Empty;
      var userName = string.Empty;

      if (!string.IsNullOrEmpty(userId))
      {
         userName = await _identityService.GetUserNameAsync(userId);
      }

      _logger.LogInformation("invensys.iserve Request: {Name} {@UserId} {@UserName} {@Request}",
          requestName, userId, userName, request);
   }
}
