namespace invensys.iserve.Infrastructure.Config;

public class ClientsConfig
{
   public List<IserveClient> IserveClients { get; set; } = [];
}

public class IserveClient
{
   public string? ClientName { get; set; }
   public string? ClientId { get; set; }
   public string? ClientSecret { get; set; }
   public string? ClientUrl { get; set; }
}
