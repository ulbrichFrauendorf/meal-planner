using MealPlanner.Application.Reports.Queries;
using MealPlanner.Infrastructure.Data;
using static MealPlanner.Application.FunctionalTests.Testing;

namespace MealPlanner.Application.FunctionalTests.Reports.Queries;

public class GetPermittedReportsTest : BaseTestFixture
{
	[Test]
	public async Task ShouldReturnAfripakPaySpaceReports()
	{
		RunAsDefaultUserAsync();

		var query = new GetOrganizationPermittedReportsQuery()
		{
			IntegratedApiId = Constants.IntegratedApiPaySpaceId,
			OrganizationId = Constants.OrganizationAfripakId,
		};

		var result = await SendAsync(query);

		result.Should().HaveCount(1);
		result.First().Description.Should().Be("Old Mutual Import Report");
		result.First().Name.Should().Be("Old Mutual Import");
	}
}
