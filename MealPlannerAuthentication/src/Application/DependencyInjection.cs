using System.Reflection;
using invensys.iserve.Application.Common.Behaviours;
using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Application.Common.Security;
using Microsoft.Extensions.DependencyInjection;

namespace invensys.iserve.Application;
public static class DependencyInjection
{
   public static IServiceCollection AddApplicationServices(this IServiceCollection services)
   {
      services.AddAutoMapper(Assembly.GetExecutingAssembly());

      services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

      services.AddMediatR(cfg =>
      {
         cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
         cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
         cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
         cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
         cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
      });

      services.AddScoped<ILocalServerHttpContextAccessor, LocalServerHttpContextAccessor>();

      return services;
   }
}
