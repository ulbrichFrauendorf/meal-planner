using static MealPlanner.Infrastructure.IntegrationTests.Testing;

namespace MealPlanner.Infrastructure.IntegrationTests.PaySpace;

internal class PaySpacePayslipApiTest : BaseTestFixture
{
	[Test]
	public async Task ShouldReturnEmployeePayslip()
	{
		var tokenResponse = await GetPaySpaceAuthTokenResponse();
		var list = await IPaySpacePayslipApi()
			.GetEmployeePayslipsAsync(tokenResponse.Token, tokenResponse.CompanyIds[0], 2024, 3);

		list.Should().NotBeNull();
		list.Should().NotBeEmpty();
		list.Should().HaveCountGreaterThanOrEqualTo(10);

		WriteTestResultsToExcel(list);
	}

	[Test]
	public async Task ShouldReturnConsolidatedEmployeePayslip()
	{
		var tokenResponse = await GetPaySpaceAuthTokenResponse();
		var list = await IPaySpacePayslipApi()
			.GetConsolidatedEmployeePayslipsAsync(
				tokenResponse.Token,
				tokenResponse.CompanyIds[0],
				2024,
				3
			);

		list.Should().NotBeNull();
		list.Should().NotBeEmpty();
		list.Should().HaveCountGreaterThanOrEqualTo(10);

		WriteTestResultsToExcel(list);
	}
}
