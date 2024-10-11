namespace invensys.iserve.Domain.Entities;

public class AngularSettings
{
   public required string Authority { get; set; }
   public required string ClientId { get; set; }
   public required string Scope { get; set; }
   public required string SpaClientUrl { get; set; }
}
