namespace MealPlanner.Infrastructure.Config;

public class TracingConfiguration
{
	public required string HostingMeterName { get; set; }
	public required string KestrelMeterName { get; set; }
	public required string HttpMeterName { get; set; }
	public required string OTLPEndpointUrl { get; set; }
}
