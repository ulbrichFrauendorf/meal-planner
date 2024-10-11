using invensys.iserve.Domain.Entities;

namespace invensys.iserve.Application.Identity.Users;

public class IdentityUserDto
{
   public string? Id { get; set; }
   public string? UserName { get; set; }
   public string? Email { get; set; }
   private class Mapping : Profile
   {
      public Mapping()
      {
         CreateMap<ApplicationUser, IdentityUserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
      }
   }
}
