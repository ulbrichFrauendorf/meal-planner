using MealPlanner.Application.Common.Exceptions;
using MealPlanner.Application.RecipeIngredients.Queries.GetRecipeIngredients;
using static MealPlanner.Application.FunctionalTests.Testing;

namespace MealPlanner.Application.FunctionalTests.RecipeIngredients.Queries;

public class GetRecipeIngredientsQueryTest : BaseTestFixture
{

	// Todo This test will fail. Need to add the iserve application to the docker container and send requests. Also registered only the front-end
	//client in the token server. Need to add config for the backend and run request contexts seperately. (Also can Use BFF strat for Duende, but licence is required)

	[Test]
	public async Task ShouldReturnAllListsAndItems()
	{
		RunAsDefaultUser();

		var query = new GetRecipeIngredientsQuery();

		var results = await SendAsync(query);

		results.Should().HaveCountGreaterThan(1);
	}

	[Test]
	public async Task ShouldDenyAnonymousUser()
	{
		var query = new GetRecipeIngredientsQuery();

		var action = () => SendAsync(query);

		await action.Should().ThrowAsync<FrontEndApiException>();
	}
}
