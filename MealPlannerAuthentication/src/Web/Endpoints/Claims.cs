using System.Security.Claims;
using invensys.iserve.Application.Identity.Claims.Commands;
using invensys.iserve.Application.Identity.Claims.Queries;
using Microsoft.AspNetCore.Mvc;

namespace invensys.iserve.Web.Endpoints;

public class Claims : EndpointGroupBase
{
   public override void Map(WebApplication app)
   {
      app.MapGroup(this).RequireAuthorization()
         .MapGet(HasClaim)
         .MapGet(GetUserClaims, "user")
         .MapPost(AddClaim, "add")
         .MapPost(RemoveClaim, "remove");
   }

   public async Task<bool> HasClaim(ISender sender, [AsParameters] HasClaimQuery query)
   {
      return await sender.Send(query);
   }

   public async Task<List<Claim>> GetUserClaims(ISender sender, [AsParameters] GetUserClaimsQuery query)
   {
      return await sender.Send(query);
   }

   public async Task<IResult> AddClaim(ISender sender, [FromBody] AddClaimCommand command)
   {
      await sender.Send(command);

      return Results.NoContent();
   }

   public async Task<IResult> RemoveClaim(ISender sender, [FromBody] RemoveClaimCommand command)
   {
      await sender.Send(command);

      return Results.NoContent();
   }
}
