﻿namespace invensys.iserve.Infrastructure.Config;

public class JwtConfig
{
   public required string Issuer { get; set; }
   public required string Audience { get; set; }
   public required string Secret { get; set; }
}