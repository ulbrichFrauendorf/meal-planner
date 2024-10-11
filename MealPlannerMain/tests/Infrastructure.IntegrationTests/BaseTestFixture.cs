namespace MealPlanner.Infrastructure.IntegrationTests;

using static MealPlanner.Infrastructure.IntegrationTests.Testing;

[TestFixture]
public abstract class BaseTestFixture
{
	[SetUp]
	public async Task TestSetUp()
	{
		await ResetState();
	}
}
