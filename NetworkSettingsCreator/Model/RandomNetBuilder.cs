using System;
using System.Collections.Generic;
using System.Linq;
using NetworkSettingsCreator.Configuration;

namespace NetworkSettingsCreator.Model
{
	class RandomNetBuilder
	{
		private readonly Random _rnd;

		public RandomNetBuilder()
		{
			_rnd = new Random();
		}

		public Node[] Build(NetworkConfiguration configuration)
		{
			var connectionsCount = GetConnectionsCount(configuration);

			var nodes = Enumerable.Range(0, (int) configuration.NodesCount).Select(x => new Node(x)).ToArray();
			var connections = Enumerable.Range(0, (int) connectionsCount).Select(x => new Connection());

			foreach (var connection in connections)
			{
				var nodeA = GetRandomNode(nodes, configuration.NodeConfiguration.ConnectionsRange.Max);
				Node nodeB;
				do
				{
					nodeB = GetRandomNode(nodes, configuration.NodeConfiguration.ConnectionsRange.Max);
				} while (nodeA.Connections.Any(c => c.Nodes.Contains(nodeB)) || nodeA == nodeB);

				connection.Nodes = new List<Node> {nodeA, nodeB};
				nodeA.Connections.Add(connection);
				nodeB.Connections.Add(connection);
			}

			return nodes;
		}

		private Node GetRandomNode(IReadOnlyList<Node> nodes, in long connectionsRangeMax)
		{
			int randomIndex;
			do
			{
				randomIndex = _rnd.Next(0, nodes.Count - 1);
			} while (nodes[randomIndex].Connections.Count >= connectionsRangeMax);

			return nodes[randomIndex];
		}

		private static long GetConnectionsCount(NetworkConfiguration configuration)
		{
			var connectionsRange = configuration.NodeConfiguration.ConnectionsRange;
			return (connectionsRange.Min + connectionsRange.Max) * configuration.NodesCount / 4;
		}
	}
}