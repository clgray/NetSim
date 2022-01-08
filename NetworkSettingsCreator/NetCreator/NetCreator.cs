using System;
using System.Linq;
using NetSim.Model;
using NetSim.Model.Connection;
using NetSim.Model.Message;
using NetSim.Model.Node;
using NetworkSettingsCreator.Configuration;
using NetworkSettingsCreator.Model;
using Range = NetworkSettingsCreator.Configuration.Range;

namespace NetworkSettingsCreator.NetCreator
{
	public class NetCreator : INetCreator
	{
		private readonly Random _rnd;

		public NetCreator()
		{
			_rnd = new Random();
		}

		public NetworkSettings CreateNet(NetworkConfiguration configuration)
		{
			var nodes = CreateNodes(configuration);
			var networkSettings = new NetworkSettings
			{
				NodeSettings = nodes.Select(n => new NodeSettings
				{
					Id = n.Id.ToString(),
					Throughput = GetRandomFromRange(configuration.NodeConfiguration.Throughput),
					//RoutingAlgorithm = configuration.NodeConfiguration.RoutingAlgorithm
				}).ToList(),
				ConnectionSettings = nodes.SelectMany(node => node.Connections
						.Where(n => n.Nodes[0].Id == node.Id)
						.Select(c => new ConnectionSettings
						{
							NodeIds = c.Nodes.Select(x => x.Id.ToString()).ToList(),
							Bandwidth = GetRandomFromRange(configuration.ConnectionConfiguration.Bandwidth),
							TimeUntilShutdown = configuration.ConnectionConfiguration.TimeUntilShutdown
						}))
					.ToList(),
				MessagesSettings = new MessagesSettings()
				{
					MaxSize = (int) configuration.MessagesConfiguration.Size.Max,
					MinSize = (int) configuration.MessagesConfiguration.Size.Min,
					Quantity = configuration.MessagesConfiguration.Quantity,
					Seed = configuration.MessagesConfiguration.Seed
				},
				TimeDelta = configuration.TimeDelta,
				SimulationSettings = new SimulationSettings()
				{ RoutingAlgorithm = configuration.NodeConfiguration.RoutingAlgorithm }
			};
			return networkSettings;
		}

		private Node[] CreateNodes(NetworkConfiguration configuration)
		{
			return configuration.NetworkType.ToLower() switch
			{
				"random" => CreateRandomNet(configuration),
				_ => throw new NotImplementedException()
			};
		}

		private static Node[] CreateRandomNet(NetworkConfiguration configuration)
		{
			var netBuilder = new RandomNetBuilder();
			return netBuilder.Build(configuration);
		}

		private int GetRandomFromRange(Range range)
		{
			return _rnd.Next((int) range.Min, (int) (range.Max));
		}
	}
}