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
			var connections = Enumerable.Range(0, (int) connectionsCount).Select(x => new Connection()).ToList();

			foreach (var connection in connections)
			{
				var node = GetRandomNode(nodes, configuration.NodeConfiguration.ConnectionsRange.Max);
				ConnectWithRandomNode(nodes, configuration.NodeConfiguration.ConnectionsRange.Max, node, connection);
			}


			var notConnectedNodes = nodes.Where(x => x.Connections.Count == 0).ToArray();
			while (notConnectedNodes.Any())
			{
				foreach (var node in notConnectedNodes)
				{
					var connection = new Connection();
					connections.Add(connection);
					ConnectWithRandomNode(nodes, configuration.NodeConfiguration.ConnectionsRange.Max, node,
						connection);
				}
				notConnectedNodes = nodes.Where(x => x.Connections.Count == 0).Select(x => x).ToArray();
			}

			return nodes;
		}

		private void ConnectWithRandomNode(Node[] nodes, long connectionsRangeMax, Node nodeA, Connection connection)
		{
			Node nodeB;
			do
			{
				nodeB = GetRandomNode(nodes, connectionsRangeMax);
			} while (nodeA.Connections.Any(c => c.Nodes.Contains(nodeB)) || nodeA == nodeB);

			connection.Nodes = new List<Node> {nodeA, nodeB};
			nodeA.Connections.Add(connection);
			nodeB.Connections.Add(connection);
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