using static MealPlanner.Infrastructure.IntegrationTests.Testing;

namespace MealPlanner.Infrastructure.IntegrationTests.PaySpace;

internal class PaySpaceEmployeeApiTest : BaseTestFixture
{
	[Test]
	public async Task ShouldReturnEmployeeList()
	{
		var tokenResponse = await GetPaySpaceAuthTokenResponse();
		var list = await IPaySpaceEmployeeApi()
			.EmployeeListAsync(
				tokenResponse.Token,
				tokenResponse.CompanyIds[0],
				DateTime.Parse("2024-03-01")
			);

		list.Should().NotBeNull();
		list.Should().NotBeEmpty();
		list.Should().HaveCountGreaterThanOrEqualTo(10);

		WriteTestResultsToExcel(list);
	}

	[Test]
	public async Task ShouldReturnEmployeeStatusList()
	{
		var tokenResponse = await GetPaySpaceAuthTokenResponse();
		var list = await IPaySpaceEmployeeApi()
			.EmployeeEmploymentStatusAsync(
				tokenResponse.Token,
				tokenResponse.CompanyIds[0],
				DateTime.Parse("2024-03-01")
			);

		list.Should().NotBeNull();
		list.Should().NotBeEmpty();
		list.Should().HaveCountGreaterThanOrEqualTo(10);

		WriteTestResultsToExcel(list);
	}

	[Test]
	public async Task ShouldReturnEmployeeBankDetailsList()
	{
		var tokenResponse = await GetPaySpaceAuthTokenResponse();
		var list = await IPaySpaceEmployeeApi()
			.EmployeeBankDetailAsync(tokenResponse.Token, tokenResponse.CompanyIds[0]);

		list.Should().NotBeNull();
		list.Should().NotBeEmpty();
		list.Should().HaveCountGreaterThanOrEqualTo(10);

		WriteTestResultsToExcel(list);
	}

	[Test]
	public async Task ShouldReturnEmployeePositionList()
	{
		var tokenResponse = await GetPaySpaceAuthTokenResponse();
		var list = await IPaySpaceEmployeeApi()
			.EmployeePositionsAsync(
				tokenResponse.Token,
				tokenResponse.CompanyIds[0],
				DateTime.Parse("2024-03-01")
			);

		list.Should().NotBeNull();
		list.Should().NotBeEmpty();
		list.Should().HaveCountGreaterThanOrEqualTo(10);

		WriteTestResultsToExcel(list);
	}
}
