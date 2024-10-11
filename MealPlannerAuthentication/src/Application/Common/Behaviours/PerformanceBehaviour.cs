using System.Diagnostics;
using invensys.iserve.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace invensys.iserve.Application.Common.Behaviours;
public class PerformanceBehaviour<TRequest, TResponse>(
    ILogger<TRequest> logger,
    IUser user,
    IIdentityUserService identityService) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
   private readonly Stopwatch _timer = new Stopwatch();

   public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
   {
      _timer.Start();

      var response = await next();

      _timer.Stop();

      var elapsedMilliseconds = _timer.ElapsedMilliseconds;

      if (elapsedMilliseconds > 500)
      {
         var requestName = typeof(TRequest).Name;
         var userId = user.Id ?? string.Empty;
         var userName = string.Empty;

         if (!string.IsNullOrEmpty(userId))
         {
            userName = await identityService.GetUserNameAsync(userId);
         }

         logger.LogWarning("invensys.iserve Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
             requestName, elapsedMilliseconds, userId, userName, request);
      }

      return response;
   }
}
