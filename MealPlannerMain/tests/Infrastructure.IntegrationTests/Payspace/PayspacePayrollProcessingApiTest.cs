using Invensys.Extensions.Data;
using static MealPlanner.Infrastructure.IntegrationTests.Testing;

namespace MealPlanner.Infrastructure.IntegrationTests.PaySpace;

internal class PaySpacePayrollProcessingApiTest : BaseTestFixture
{
	[Test]
	public async Task ShouldReturnEmployeeComponentsList()
	{
		var tokenResponse = await GetPaySpaceAuthTokenResponse();
		var listEmps = (
			await IPaySpaceEmployeeApi()
				.EmployeeListAsync(
					tokenResponse.Token,
					tokenResponse.CompanyIds[0],
					DateTime.Parse("2024-03-01")
				)
		)
			.SelectString(e => e.EmployeeNumber)
			.ToList();

		var list = await IPaySpacePayrollProcessingApi()
			.GetEmployeeComponentsAsync(
				tokenResponse.Token,
				tokenResponse.CompanyIds[0],
				"006_Salaries",
				"20243",
				listEmps
			);

		list.Should().NotBeNull();
		list.Should().NotBeEmpty();
		list.Should().HaveCountGreaterThanOrEqualTo(10);

		WriteTestResultsToExcel(list);
	}

	[Test]
	public async Task ShouldReturnEmployeePayRateList()
	{
		var tokenResponse = await GetPaySpaceAuthTokenResponse();
		var list = await IPaySpacePayrollProcessingApi()
			.EmployeePayRateAsync(
				tokenResponse.Token,
				tokenResponse.CompanyIds[0],
				DateTime.Parse("2024-03-01")
			);

		list.Should().NotBeNull();
		list.Should().NotBeEmpty();
		list.Should().HaveCountGreaterThanOrEqualTo(1);

		WriteTestResultsToExcel(list);
	}

	[Test]
	public async Task ShouldReturnEmployeePensionFundList()
	{
		var tokenResponse = await GetPaySpaceAuthTokenResponse();
		var listEmps = (
			await IPaySpaceEmployeeApi()
				.EmployeeListAsync(
					tokenResponse.Token,
					tokenResponse.CompanyIds[0],
					DateTime.Parse("2024-03-01")
				)
		).SelectString(e => e.EmployeeNumber);

		var list = await IPaySpacePayrollProcessingApi()
			.GetEmployeePensionFundAsync(
				tokenResponse.Token,
				tokenResponse.CompanyIds[0],
				"006_Salaries",
				"20243",
				listEmps
			);

		list.Should().NotBeNull();
		list.Should().NotBeEmpty();
		list.Should().HaveCountGreaterThanOrEqualTo(10);

		WriteTestResultsToExcel(list);
	}
}
