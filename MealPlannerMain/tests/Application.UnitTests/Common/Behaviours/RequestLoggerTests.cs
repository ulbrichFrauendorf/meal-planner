using MealPlanner.Application.Common.Behaviours;
using MealPlanner.Application.Common.Interfaces;
using MealPlanner.Application.ExternalApi.PayrollApi.Payspace.Reports.Commands;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace MealPlanner.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
	private Mock<ILogger<DeleteUnsuccessfulReportCommand>> _logger = null!;
	private Mock<IUser> _user = null!;
	private Mock<IIdentityService> _identityService = null!;

	[SetUp]
	public void Setup()
	{
		_logger = new Mock<ILogger<DeleteUnsuccessfulReportCommand>>();
		_user = new Mock<IUser>();
		_identityService = new Mock<IIdentityService>();
	}

	[Test]
	public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
	{
		var generatedGuid = Guid.NewGuid();
		_user.Setup(x => x.Id).Returns(generatedGuid.ToString());

		var requestLogger = new LoggingBehaviour<DeleteUnsuccessfulReportCommand>(
			_logger.Object,
			_user.Object,
			_identityService.Object
		);

		await requestLogger.Process(
			new DeleteUnsuccessfulReportCommand(generatedGuid),
			new CancellationToken()
		);

		_identityService.Verify(i => i.GetUserName(), Times.Once);
	}

	[Test]
	public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
	{
		var generatedGuid = Guid.NewGuid();
		var requestLogger = new LoggingBehaviour<DeleteUnsuccessfulReportCommand>(
			_logger.Object,
			_user.Object,
			_identityService.Object
		);

		await requestLogger.Process(
			new DeleteUnsuccessfulReportCommand(generatedGuid),
			new CancellationToken()
		);

		_identityService.Verify(i => i.GetUserName(), Times.Never);
	}
}
