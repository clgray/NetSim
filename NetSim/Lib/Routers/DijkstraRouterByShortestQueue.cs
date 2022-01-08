using System.Collections.Generic;
using System.Linq;
using NetSim.Providers;

namespace NetSim.Lib.Routers
{
	public class DijkstraRouterByShortestQueue : DijkstraRouterBase
	{
		protected override void SetNodes(IReadOnlyCollection<INode> net)
		{
			_graph = new Graph(net.Count());
			foreach (var node in net)
			{
				var neighbours = node
					.GetConnections()
					.SelectMany(x => x.GetConnectedNodes()
						.Where(x => !x.Equals(node)));
				foreach (var neighbour in neighbours)
				{
					var weight = 0;

                    var nodeMetrics = neighbour.GetNodeState();

					var connection = node.GetConnections().First(x => x.GetConnectedNodes().Any(x => x == neighbour));

					var queue = nodeMetrics.MessagesInQueueSize / nodeMetrics.Throughput + connection.TimeWaiting;
					if (queue < 5 )
						weight = 1;
					else weight = (int)queue;

					if (!ResourceProvider.SimulationSettings.UseOnlyIsActiveNodes || neighbour.IsActive())
						_graph.AddEdge(int.Parse(node.GetId()), int.Parse(neighbour.GetId()), weight);
				}
			}
		}
	}
}