namespace invensys.iserve.Application.Common.Interfaces;

public interface ILocalServerHttpContextAccessor
{
   string? TrimCurrentSiteReturnUrl(string? returnUrl);
}
