
using Duende.IdentityServer.EntityFramework.Entities;

namespace invensys.iserve.Application.Clients.Queries.GetClients;

public class ClientDto
{
   public string? ClientId { get; set; }

   public string? ProtocolType { get; set; }

   public bool RequireClientSecret { get; set; }

   public string? ClientName { get; set; }

   public string? Description { get; set; }

   public string? ClientUri { get; set; }

   public string? LogoUri { get; set; }

   public bool RequireConsent { get; set; }

   public bool AllowRememberConsent { get; set; }

   public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

   public bool RequirePkce { get; set; }

   public bool AllowPlainTextPkce { get; set; }

   public bool RequireRequestObject { get; set; }

   public bool AllowAccessTokensViaBrowser { get; set; }

   public bool RequireDPoP { get; set; }

   public int DPoPValidationMode { get; set; }

   public TimeSpan DPoPClockSkew { get; set; }

   public string? FrontChannelLogoutUri { get; set; }

   public bool FrontChannelLogoutSessionRequired { get; set; }

   public string? BackChannelLogoutUri { get; set; }

   public bool BackChannelLogoutSessionRequired { get; set; }

   public bool AllowOfflineAccess { get; set; }

   public int IdentityTokenLifetime { get; set; }

   public string? AllowedIdentityTokenSigningAlgorithms { get; set; }

   public int AccessTokenLifetime { get; set; }

   public int AuthorizationCodeLifetime { get; set; }

   public int? ConsentLifetime { get; set; }

   public int AbsoluteRefreshTokenLifetime { get; set; }

   public int SlidingRefreshTokenLifetime { get; set; }

   public int RefreshTokenUsage { get; set; }

   public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

   public int RefreshTokenExpiration { get; set; }

   public int AccessTokenType { get; set; }

   public bool EnableLocalLogin { get; set; }

   public bool IncludeJwtId { get; set; }

   public bool AlwaysSendClientClaims { get; set; }

   public string? ClientClaimsPrefix { get; set; }

   public string? PairWiseSubjectSalt { get; set; }

   public string? InitiateLoginUri { get; set; }

   public int? UserSsoLifetime { get; set; }

   public string? UserCodeType { get; set; }

   public int DeviceCodeLifetime { get; set; }

   public int? CibaLifetime { get; set; }

   public int? PollingInterval { get; set; }

   public bool? CoordinateLifetimeWithUserSession { get; set; }

   public DateTime Created { get; set; }

   public DateTime? Updated { get; set; }

   public DateTime? LastAccessed { get; set; }

   public bool NonEditable { get; set; }

   public int? PushedAuthorizationLifetime { get; set; }

   public bool RequirePushedAuthorization { get; set; }

   private class Mapping : Profile
   {
      public Mapping()
      {
         CreateMap<Client, ClientDto>();
      }
   }
}
