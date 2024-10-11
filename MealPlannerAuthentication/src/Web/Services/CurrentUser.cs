using System.Security.Claims;

using invensys.iserve.Application.Common.Interfaces;

namespace invensys.iserve.Web.Services;
public class CurrentUser : IUser
{
   private readonly IHttpContextAccessor _httpContextAccessor;

   public CurrentUser(IHttpContextAccessor httpContextAccessor)
   {
      _httpContextAccessor = httpContextAccessor;
   }

   public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");
}
