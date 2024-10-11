using invensys.iserve.Application.Identity.Users;
using invensys.iserve.Application.Identity.Users.Commands;
using invensys.iserve.Application.Identity.Users.Queries;

namespace invensys.iserve.Web.Endpoints;

public class Identities : EndpointGroupBase
{
   public override void Map(WebApplication app)
   {
      app.MapGroup(this)
         .RequireAuthorization()
         .MapGet(GetIdentityUser)
         .MapGet(GetIdentityUsers, "all")
         .MapPost(CreateIdentityUser, "create")
         .MapPost(UpdateIdentityUser,"update/{id}")
         .MapPost(DeleteIdentityUser,"delete/{id}");
   }

   public async Task<IdentityUserVM> GetIdentityUser(ISender sender, Guid userId)
   {
      return await sender.Send(new GetIdentityUserQuery { UserId = userId});
   }

   public async Task<List<IdentityUserVM>> GetIdentityUsers(ISender sender)
   {
      return await sender.Send(new GetIdentityUsersQuery());
   }

   public async Task<string> CreateIdentityUser(ISender sender, CreateIdentityUserCommand createUserCommand)
   {
      return await sender.Send(createUserCommand);
   }

   public async Task<IResult> UpdateIdentityUser(ISender sender, Guid id, UpdateIdentityUserCommand command)
   {
      if (id != command.UserId) return Results.BadRequest();

      await sender.Send(command);
      
      return Results.NoContent();
   }

   public async Task<IResult> DeleteIdentityUser(ISender sender, Guid id)
   {
      await sender.Send(new DeleteIdentityUserCommand { UserId = id });

      return Results.NoContent();
   }

}
