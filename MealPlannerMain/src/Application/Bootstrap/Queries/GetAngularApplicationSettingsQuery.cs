using MealPlanner.Domain.Configuration;
using Microsoft.Extensions.Configuration;



namespace MealPlanner.Application.Bootstrap.Queries;

public record GetAngularApplicationSettingsQuery : IRequest<AngularSettingsDto>
{
}

public class GetAngularApplicationSettingsQueryHandler(IConfiguration configuration, IMapper mapper) : IRequestHandler<GetAngularApplicationSettingsQuery, AngularSettingsDto>
{
	public async Task<AngularSettingsDto> Handle(GetAngularApplicationSettingsQuery request, CancellationToken cancellationToken)
	{
		await Task.CompletedTask;

		var angularSettings = configuration.GetSection(nameof(AngularSettings));//.Get<AngularSettings>();


		Guard.Against.Null(angularSettings, nameof(angularSettings));

		return mapper.Map<AngularSettingsDto>(angularSettings);
	}
}
