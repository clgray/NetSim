namespace NetworkSettingsCreator.Configuration
{
	public class NodeConfiguration
	{
		public string RoutingAlgorithm { get; set; }
		public Range Throughput { get; set; }
		public Range ConnectionsRange { get; set; }
		public long MessageQueueLength { get; set; }
	}
}