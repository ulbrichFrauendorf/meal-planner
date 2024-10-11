using MealPlanner.Domain.Configuration;

namespace MealPlanner.Application.Bootstrap.Queries;

public class AngularSettingsDto
{
	public required string Authority { get; set; }
	public required string ClientId { get; set; }
	public required string Scope { get; set; }

	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<AngularSettings, AngularSettingsDto>();
		}
	}
}
