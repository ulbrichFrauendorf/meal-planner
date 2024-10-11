using Invensys.Api.Common.Common;
using static MealPlanner.Infrastructure.IntegrationTests.Testing;

namespace MealPlanner.Infrastructure.IntegrationTests.PaySpace;

internal class PaySpaceReportsTest : BaseTestFixture
{
	[Test]
	public async Task ShouldReturnOldMutualReportList()
	{
		var paySpaceTestClientConfig = GetPaySpaceTestClientConfig();
		var accessTokenRequest = GetPaySpaceJwtAccessTokenRequest(paySpaceTestClientConfig);

		var report = await OldMutualImportReport()
			.GenerateExcel(
				accessTokenRequest,
				[
					new()
					{
						CompanyId = 15611,
						FrequencyValue = "006_Salaries",
						Value = "20243"
					}
				],
				[
					new ReportConfigurationProperty()
					{
						PropertyName = "PensionableSalary",
						LinkCode = "RFI_Income"
					},
					new ReportConfigurationProperty()
					{
						PropertyName = "EmployeeAdditionalVoluntaryContribution",
						LinkCode = "OM_Pens_Add_DD"
					},
					new ReportConfigurationProperty()
					{
						PropertyName = "MemberCategory",
						LinkCode = "OM_Cat"
					},
					new ReportConfigurationProperty()
					{
						PropertyName = "EmployerAdditionalVoluntaryContribution",
						LinkCode = "OM_Pens_Add_CC"
					},
				],
				DateTime.Parse("2024-03-01"),
				"OldMutualReport",
				null,
				true
			);

		report.Should().NotBeNull();
		report.Should().NotBeEmpty();

		WriteExcelTestResultsToDisk(report, "OldMutualReport");
	}

	[Test]
	public async Task ShouldReturnAlexanderForbesReportList()
	{
		var paySpaceTestClientConfig = GetPaySpaceTestClientConfig();
		var accessTokenRequest = GetPaySpaceJwtAccessTokenRequest(paySpaceTestClientConfig);

		var report = await AlexanderForbesImportReport()
			.GenerateExcel(
				accessTokenRequest,
				[
					new()
					{
						CompanyId = int.Parse(paySpaceTestClientConfig.CompanyId),
						FrequencyValue = "006_Salaries",
						Value = "20246"
					}
				],
				[
					new ReportConfigurationProperty()
					{
						PropertyName = "PensionableSalary",
						LinkCode = "RFI_Income"
					},
					new ReportConfigurationProperty()
					{
						PropertyName = "EmployeeAdditionalVoluntaryContribution",
						LinkCode = "AF_Pens_Add_DD"
					},
					new ReportConfigurationProperty()
					{
						PropertyName = "MemberCategory",
						LinkCode = "AF_Cat"
					},
					new ReportConfigurationProperty()
					{
						PropertyName = "EmployerAdditionalVoluntaryContribution",
						LinkCode = "AF_Pens_Add_CC"
					},
				],
				DateTime.Parse("2024-03-01"),
				"AlexanderForbesImportReport",
				null,
				true
			);

		report.Should().NotBeNull();
		report.Should().NotBeEmpty();

		WriteExcelTestResultsToDisk(report, "OldMutualReport");
	}

	[Test]
	public async Task ShouldReturnParollSmmaryReport()
	{
		var paySpaceTestClientConfig = GetPaySpaceTestClientConfig();
		var accessTokenRequest = GetPaySpaceJwtAccessTokenRequest(paySpaceTestClientConfig);

		var report = await PayrollSummaryReport()
			.GenerateExcel(accessTokenRequest
				,
				[
					new()
					{

						CompanyId = int.Parse(paySpaceTestClientConfig.CompanyId),
						FrequencyValue = paySpaceTestClientConfig.FrequencyValue,
						Value = paySpaceTestClientConfig.PayrunValue,
						Description= paySpaceTestClientConfig.PayrunDescription
					},
				],
				[],
				DateTime.Parse("2024-09-01"),
				"ParollSummary.xlsx",
				"",
				true
			);

		report.Should().NotBeNull();
		report.Should().NotBeEmpty();

		WriteExcelTestResultsToDisk(report, "ParollSummary");
	}

	[Test]
	public async Task ShouldReturnHeadCountReport()
	{
		var paySpaceTestClientConfig = GetPaySpaceTestClientConfig();
		var accessTokenRequest = GetPaySpaceJwtAccessTokenRequest(paySpaceTestClientConfig);

		var report = await HeadcountReport()
			.GenerateExcel(accessTokenRequest
				,
				[
					new()
					{

						CompanyId = int.Parse(paySpaceTestClientConfig.CompanyId),
						CompanyDescription = paySpaceTestClientConfig.CompanyDescription,
						FrequencyValue = paySpaceTestClientConfig.FrequencyValue,
						Value = paySpaceTestClientConfig.PayrunValue,
						Description= paySpaceTestClientConfig.PayrunDescription
					},
				],
				[],
				DateTime.Parse("2024-09-01"),
				"Headcount.xlsx",
				"Department",
				true
			);

		report.Should().NotBeNull();
		report.Should().NotBeEmpty();

		WriteExcelTestResultsToDisk(report, "Headcount");
	}
}
