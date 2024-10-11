using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Domain.Constants;
using invensys.iserve.Domain.Entities;
using invensys.iserve.Infrastructure.Config;
using invensys.iserve.Infrastructure.Data;
using invensys.iserve.Infrastructure.Data.Interceptors;
using invensys.iserve.Infrastructure.Email;
using invensys.iserve.Infrastructure.Identity;
using invensys.iserve.Infrastructure.Profile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace invensys.iserve.Infrastructure;

public static class DependencyInjection
{
   public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
      IConfiguration configuration,
      IWebHostEnvironment environment)
   {
      var connectionString = configuration.GetConnectionString("DefaultConnection");

      Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

      services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
      services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

      services.AddDbContext<ApplicationDbContext>((sp, options) =>
      {
         options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

         options.UseSqlServer(connectionString);
      });

      services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

      services.AddScoped<ApplicationDbContextInitialiser>();
      services.AddScoped<ConfigurationDbContextInitialiser>();

      var jwtConfig = configuration.GetSection(nameof(JwtConfig)).Get<JwtConfig>();
      services.AddLocalApiAuthentication();
      services.AddAuthentication(options =>
      {
         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
         options.Authority = jwtConfig!.Issuer;
         options.Audience = jwtConfig!.Audience;
         options.RequireHttpsMetadata = true;

         options.TokenValidationParameters = new TokenValidationParameters
         {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
         };
      });

      services.AddAuthorizationBuilder()
         .AddPolicy(Policies.IserveAdmin, policy => policy.RequireClaim(ClaimTypes.Role, RoleClaim.IserveAdministrator));

      services
         .AddIdentity<ApplicationUser, IdentityRole>()
         .AddEntityFrameworkStores<ApplicationDbContext>()
         .AddDefaultTokenProviders();

      var identityServerConfig = configuration.GetSection(nameof(IdentityServerConfig)).Get<IdentityServerConfig>();

      Guard.Against.Null(identityServerConfig, nameof(identityServerConfig), "Configuration missing for IdentityServerConfig");

      var rsaCert =
         new X509Certificate2(Properties.Resources.duende_key, identityServerConfig.CertificatePass, X509KeyStorageFlags.MachineKeySet);
      Guard.Against.Null(rsaCert, nameof(rsaCert), message: "No cert found in resource.");

      services.AddIdentityServer(options =>
      {

         options.UserInteraction.LoginUrl = environment.IsDevelopment() ? "https://localhost:44448/auth/login" : "/auth/login";

         options.EmitStaticAudienceClaim = true;
         options.KeyManagement.Enabled = false;
      })
         .AddConfigurationStore(options => options.ConfigureDbContext = builder =>
            builder.UseSqlServer(connectionString,
               sql => sql.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)))
         .AddSigningCredential(rsaCert, SecurityAlgorithms.RsaSha256)
         .AddAspNetIdentity<ApplicationUser>()
         .AddProfileService<ClaimsProfileService>();

      services.AddSingleton(TimeProvider.System);
      services.AddTransient<IIdentityUserService, IdentityUserService>();
      services.AddTransient<IIdentityAuthorizationService, IdentityAuthorizationService>();
      services.AddTransient<IIdentityRoleClaimService, IdentityRoleClaimService>();
      services.AddTransient<IIdentitySystemClaimService, IdentitySystemClaimService>();
      services.AddTransient<IEmailService, EmailService>();
      services.AddSmtpClient(configuration);

      services.AddCors(options =>
      {
         options.AddPolicy("AllowAllMethods", options => options.AllowAnyMethod());
         options.AddPolicy(
            "AllowAngularOrigin",
            builder => builder
                  .WithOrigins("https://localhost:44448", "https://iserve.invensys.web.za")
                  .AllowAnyHeader()
                  .AllowAnyMethod());
      });

      return services;
   }
}

public static class SmtpClientExtensions
{
   public static IServiceCollection AddSmtpClient(this IServiceCollection services, IConfiguration configuration)
   {
      var emailConfiguration = configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();

      Guard.Against.Null(emailConfiguration, message: "Email configuration not found.");

      services.AddTransient(sp =>
      {
         var smtpClient = new SmtpClient(emailConfiguration.SmtpHost, emailConfiguration.SmtpPort)
         {
            Credentials = new NetworkCredential(emailConfiguration.SmtpUsername, emailConfiguration.SmtpPassword),
            EnableSsl = emailConfiguration.EnableSsl
         };
         return smtpClient;
      });

      return services;
   }
}
