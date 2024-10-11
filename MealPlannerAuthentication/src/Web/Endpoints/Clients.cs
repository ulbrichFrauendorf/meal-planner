using invensys.iserve.Application.Clients.Queries.GetClients;

namespace invensys.iserve.Web.Endpoints;

public class Clients : EndpointGroupBase
{
   public override void Map(WebApplication app)
   {
      app.MapGroup(this)
         .RequireAuthorization()
         .MapGet(GetClients);
   }

   public async Task<List<ClientDto>> GetClients(ISender sender)
   {
      return await sender.Send(new GetClientsQuery());
   }
}
