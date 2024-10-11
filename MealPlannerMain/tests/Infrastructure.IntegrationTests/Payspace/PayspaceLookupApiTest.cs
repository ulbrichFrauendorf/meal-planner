using Invensys.Extensions.Data;
using static MealPlanner.Infrastructure.IntegrationTests.Testing;

namespace MealPlanner.Infrastructure.IntegrationTests.PaySpace;

internal class PaySpaceLookupApiTest : BaseTestFixture
{
	[Test]
	public async Task ShouldReturnCompanyFrequencyList()
	{
		var tokenResponse = await GetPaySpaceAuthTokenResponse();
		var list = await IPaySpaceCompanyApi()
			.GetCompanyFrequenciesAsync(tokenResponse.Token, tokenResponse.CompanyIds[0]);

		list.Should().NotBeNull();
		list.Should().NotBeEmpty();
		list.Should().HaveCountGreaterThanOrEqualTo(1);

		WriteTestResultsToExcel(list);
	}

	[Test]
	public async Task ShouldReturnCompanyPensionFundList()
	{
		var tokenResponse = await GetPaySpaceAuthTokenResponse();
		var list = await IPaySpaceLookupApi()
			.GetCompanyPensionFundAsync(tokenResponse.Token, tokenResponse.CompanyIds[0]);

		list.Should().NotBeNull();
		list.Should().NotBeEmpty();
		list.Should().HaveCountGreaterThanOrEqualTo(1);

		WriteTestResultsToExcel(list);
	}

	[Test]
	public async Task ShouldReturnCompanyRunsList()
	{
		var tokenResponse = await GetPaySpaceAuthTokenResponse();
		var list = await IPaySpaceCompanyApi()
			.GetCompanyRunsAsync(tokenResponse.Token, tokenResponse.CompanyIds[0], "006_Salaries");

		list.Should().NotBeNull();
		list.Should().NotBeEmpty();
		list.Should().HaveCountGreaterThanOrEqualTo(10);

		WriteTestResultsToExcel(list);
	}

	[Test]
	public async Task ShouldReturnNatureOfPersonList()
	{
		var tokenResponse = await GetPaySpaceAuthTokenResponse();
		var list = await IPaySpaceLookupApi()
			.GetNatureOfPersonAsync(tokenResponse.Token, tokenResponse.CompanyIds[0]);

		list.Should().NotBeNull();
		list.Should().NotBeEmpty();
		list.Should().HaveCountGreaterThanOrEqualTo(10);

		WriteTestResultsToExcel(list);
	}

	[Test]
	public async Task ShouldReturnComponentCompanyDetailList()
	{
		var tokenResponse = await GetPaySpaceAuthTokenResponse();

		var list = await IPaySpaceCompanyApi()
			.GetComponentCompanyDetailAsync(
				tokenResponse.Token,
				tokenResponse.CompanyIds[0],
				"006_Salaries",
				"20243"
			);

		list.Should().NotBeNull();
		list.Should().NotBeEmpty();
		list.Should().HaveCountGreaterThanOrEqualTo(10);

		WriteTestResultsToExcel(list);
	}

	[Test]
	public async Task ShouldReturnCompanyPensionFundLinkList()
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

		var listPs = await IPaySpacePayrollProcessingApi()
			.GetEmployeePensionFundAsync(
				tokenResponse.Token,
				tokenResponse.CompanyIds[0],
				"006_Salaries",
				"20243",
				listEmps
			);

		var st = listPs.SelectString(e => e.CompanyPensionFundLink).Distinct();

		var list = await IPaySpaceLookupApi()
			.GetCompanyPensionFundLinkAsync(
				tokenResponse.Token,
				tokenResponse.CompanyIds[0],
				"006_Salaries",
				"20243",
				st
			);

		list.Should().NotBeNull();
		list.Should().NotBeEmpty();
		list.Should().HaveCountGreaterThanOrEqualTo(10);

		WriteTestResultsToExcel(list);
	}
}
