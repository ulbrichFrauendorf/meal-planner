﻿namespace MealPlanner.Application.FunctionalTests;

[TestFixture]
public abstract class BaseTestFixture
{
	[SetUp]
	public async Task TestSetUp()
	{
		await Task.CompletedTask;
	}
}
