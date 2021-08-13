namespace NetworkSettingsCreator.Configuration
{
	public class NetworkConfiguration
	{
		public float TimeDelta { get; set; }
		public string NetworkType { get; set; }
		public long NodesCount { get; set; }
		public NodeConfiguration NodeConfiguration { get; set; }
		public ConnectionConfiguration ConnectionConfiguration { get; set; }
		public MessagesConfiguration MessagesConfiguration { get; set; }
	}
}