using invensys.iserve.Application;
using invensys.iserve.Infrastructure;
using invensys.iserve.Infrastructure.Data;
using invensys.iserve.Web;
using Sentry.OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKeyVaultIfConfigured(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment);
builder.Services.AddWebServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   await app.InitialiseDatabaseAsync();
}
else
{
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.UseSwaggerUi(settings =>
{
   settings.Path = "/api";
   settings.DocumentPath = "/api/specification.json";
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages().RequireAuthorization();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });


app.MapEndpoints();

SentrySdk.CaptureMessage($"iServe Environment: {app.Environment.EnvironmentName}");

app.Run();

namespace invensys.iserve.Web
{
   public partial class Program { }
}
