namespace invensys.iserve.Application.Identity.Users;

public class IdentityUserVM
{
   public IdentityUserDto? IdentityUser { get; set; }
   public List<string> RoleClaims { get; set; } = [];
   public List<string> SystemClaims { get; set; } = [];
}
