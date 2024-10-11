using Ardalis.GuardClauses;
using Invensys.Api.Builder;
using Invensys.Api.Common.Common.Auth;
using Invensys.Api.PaySpace.Interfaces;
using Invensys.Api.PaySpace.Reports.Core.Base.Interfaces;
using Invensys.Api.PaySpace.Reports.ExcelData.Generic.AlexanderForbesImport.AlexanderForbesPensionImport;
using Invensys.Api.PaySpace.Reports.ExcelData.Generic.Headcount;
using Invensys.Api.PaySpace.Reports.ExcelData.Generic.OldMutualImport;
using Invensys.Api.PaySpace.Reports.ExcelData.Generic.PayrollSummary;
using Invensys.Extensions.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MealPlanner.Infrastructure.IntegrationTests;

[SetUpFixture]
public partial class Testing
{
	private static CustomWebApplicationFactory s_factory = null!;
	private static IServiceScopeFactory s_scopeFactory = null!;

	[OneTimeSetUp]
	public async Task RunBeforeAnyTests()
	{
		s_factory = new CustomWebApplicationFactory();

		s_scopeFactory = s_factory.Services.GetRequiredService<IServiceScopeFactory>();

		await Task.CompletedTask.ConfigureAwait(false);
	}

	public static PaySpaceTestClientConfig GetPaySpaceTestClientConfig()
	{
		var scope = s_factory.Services.CreateScope();

		var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

		var paySpaceTestClientConfig = config.GetSection(nameof(PaySpaceTestClientConfig)).Get<PaySpaceTestClientConfig>();

		Guard.Against.Null(paySpaceTestClientConfig, nameof(paySpaceTestClientConfig));

		return paySpaceTestClientConfig;
	}

	public static JwtAccessTokenRequest GetPaySpaceJwtAccessTokenRequest(PaySpaceTestClientConfig paySpaceTestClientConfig)
	{
		return new JwtAccessTokenRequest(
			paySpaceTestClientConfig.ClientId,
			paySpaceTestClientConfig.ClientSecret,
			paySpaceTestClientConfig.Scope
		);
	}

	public static async Task<(string Token, long[] CompanyIds)> GetPaySpaceAuthTokenResponse()
	{
		using var scope = s_scopeFactory.CreateScope();

		var payspaceClient =
			scope.ServiceProvider.GetRequiredService<IPaySpaceAuthenticationProvider>();

		var paySpaceTestClientConfig = GetPaySpaceTestClientConfig();
		var accessTokenRequest = GetPaySpaceJwtAccessTokenRequest(paySpaceTestClientConfig);

		var authenticationResult = await payspaceClient.GetAuthenticationResult(accessTokenRequest);

		var accessToken = await payspaceClient.GetAccessToken(accessTokenRequest);

		return (
			accessToken,
			authenticationResult
				.AuthenticationResultData!.Select(s => s.Companies)
				.First()!
				.Select(s => s.CompanyId)
				.ToArray()
		);
	}

	public static IPaySpaceEmployeeApi IPaySpaceEmployeeApi()
	{
		using var scope = s_scopeFactory.CreateScope();

		return scope.ServiceProvider.GetRequiredService<IPaySpaceEmployeeApi>();
	}

	public static IPaySpacePayslipApi IPaySpacePayslipApi()
	{
		using var scope = s_scopeFactory.CreateScope();

		return scope.ServiceProvider.GetRequiredService<IPaySpacePayslipApi>();
	}

	public static IPaySpacePayrollProcessingApi IPaySpacePayrollProcessingApi()
	{
		using var scope = s_scopeFactory.CreateScope();

		return scope.ServiceProvider.GetRequiredService<IPaySpacePayrollProcessingApi>();
	}

	public static IPaySpaceLookupApi IPaySpaceLookupApi()
	{
		using var scope = s_scopeFactory.CreateScope();

		return scope.ServiceProvider.GetRequiredService<IPaySpaceLookupApi>();
	}
	public static IPaySpaceCompanyApi IPaySpaceCompanyApi()
	{
		using var scope = s_scopeFactory.CreateScope();

		return scope.ServiceProvider.GetRequiredService<IPaySpaceCompanyApi>();
	}
	
	public static IPaySpaceReport OldMutualImportReport()
	{
		using var scope = s_scopeFactory.CreateScope();

		return scope.ServiceProvider.GetRequiredService<OldMutualImportReport>();
	}

	public static IPaySpaceReport PayrollSummaryReport()
	{
		using var scope = s_scopeFactory.CreateScope();

		return scope.ServiceProvider.GetRequiredService<PayrollSummaryReport>();
	}

	public static IPaySpaceReport AlexanderForbesImportReport()
	{
		using var scope = s_scopeFactory.CreateScope();

		return scope.ServiceProvider.GetRequiredService<AlexanderForbesPensionImportReport>();
	}

	public static IPaySpaceReport HeadcountReport()
	{
		using var scope = s_scopeFactory.CreateScope();

		return scope.ServiceProvider.GetRequiredService<HeadcountReport>();
	}

	public static void WriteTestResultsToExcel<T>(IEnumerable<T> list)
	{
		var excelBytes = list.ToExcelByteArray("TestSheet");
		WriteExcelTestResultsToDisk(excelBytes, typeof(T).Name + "List.xlsx");
	}

	public static void WriteExcelTestResultsToDisk(byte[] excelBytes, string filename)
	{
		var testDir = Path.Combine(Environment.CurrentDirectory, "test-sheets");
		Directory.CreateDirectory(testDir);
		File.WriteAllBytes(
			Path.Combine(Environment.CurrentDirectory, "test-sheets", filename + ".zip"),
			excelBytes
		);
	}

	public static async Task ResetState()
	{
		await Task.CompletedTask.ConfigureAwait(false);
	}

	[OneTimeTearDown]
	public async Task RunAfterAnyTests()
	{
		await s_factory.DisposeAsync();
	}
}
