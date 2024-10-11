using MealPlanner.Application;
using MealPlanner.Infrastructure;
using MealPlanner.Infrastructure.Data;
using MealPlanner.Web;
using Sentry.OpenTelemetry;

//Todo Use specs
//Todo Use events

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry(options =>
{
	options.UseOpenTelemetry();
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(
	builder.Configuration,
	builder.Environment
);
builder.Services.AddWebServices();

builder.Services.AddHsts(options =>
{
	options.Preload = true;
	options.IncludeSubDomains = true;
	options.MaxAge = TimeSpan.FromDays(365);
});

builder.Services.AddSentryTunneling();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
}

await app.InitialiseDatabaseAsync();

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi(settings =>
{
	settings.Path = "/api";
	settings.DocumentPath = "/api/specification.json";
	settings.PersistAuthorization = true;
});
app.UseReDoc(options =>
{
	options.Path = "/redoc";
});

app.UseCors("AllowAngularOrigin");

app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });

app.MapEndpoints();

app.Run();

namespace MealPlanner.Web
{
	public partial class Program { }
}
