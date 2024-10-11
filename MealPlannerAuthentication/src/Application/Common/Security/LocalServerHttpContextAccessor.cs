using invensys.iserve.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace invensys.iserve.Application.Common.Security
{
   public class LocalServerHttpContextAccessor(IHttpContextAccessor httpContextAccessor, IHostEnvironment environment) : ILocalServerHttpContextAccessor
   {
      public string? TrimCurrentSiteReturnUrl(string? returnUrl)
      {
         if (environment.IsDevelopment())
         {
            var request = httpContextAccessor.HttpContext?.Request;

            Guard.Against.Null(request);

            var hostUrl = $"{request.Scheme}://localhost:5002";

            return returnUrl?.Replace(hostUrl, "");
         }

         return returnUrl;
      }
   }
}
