using FluentAssertions;
using NUnit.Framework;

namespace MealPlanner.Domain.UnitTests.ValueObjects;

public class ColourTests
{
	[Test]
	public void ToStringReturnsCode()
	{
		var colour = "300";

		colour.ToString().Should().Be("300");
	}
}
