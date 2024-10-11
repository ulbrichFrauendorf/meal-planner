using invensys.iserve.Application.Account.Commands;
using invensys.iserve.Application.Account.Queries;
using invensys.iserve.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace invensys.iserve.Web.Endpoints;

public class Accounts : EndpointGroupBase
{
   public override void Map(WebApplication app)
   {
      app.MapGroup(this)
         .MapGet(GetLoginData, "login")
         .MapPost(Login)
         .MapPost(ChangePassword, "change-password")
         .MapPost(ForgotPassword, "forgot-password")
         .MapPost(ResetPassword, "reset-password")
         ;
   }

   public async Task<LoginDto> GetLoginData(ISender sender, string returnUrl)
   {
      return await sender.Send(new GetLoginDataQuery() { RedirectUrl = returnUrl });
   }

   public async Task<LoginResponse> Login(ISender sender, [FromBody] LoginCommand command)
   {

      return await sender.Send(command);
   }

   public async Task<IResult> ChangePassword(ISender sender, ChangePasswordCommand changeUserPasswordCommand)
   {
      await sender.Send(changeUserPasswordCommand);

      return Results.NoContent();
   }

   public async Task<IResult> ForgotPassword(ISender sender, ForgotPasswordCommand generatePasswordResetEmailCommand)
   {
      await sender.Send(generatePasswordResetEmailCommand);

      return Results.NoContent();
   }

   public async Task<IResult>ResetPassword(ISender sender, ResetPasswordCommand resetPasswordCommand)
   {
      await sender.Send(resetPasswordCommand);

      return Results.NoContent();
   }

   
}
