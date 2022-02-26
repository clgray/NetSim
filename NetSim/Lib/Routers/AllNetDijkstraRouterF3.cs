using System.Collections.Generic;
using System.Linq;
using NetSim.Lib.Routers.Percolation;
using NetSim.Providers;

namespace NetSim.Lib.Routers
{
	public class AllNetDijkstraRouterF3 : DijkstraRouterBase
	{
		protected override void SetNodes(IReadOnlyCollection<INode> net)
		{
			_graph = new Graph(net.Count);
			var ε = ResourceProvider.BlockedNodesByStep;
			var ξ = ResourceProvider.UnBlockedNodesByStep;
			var blocked = net.Count(x => !x.IsActive());
			var x0 = blocked / (double)net.Count;
			var L = 1;
			var t = 1000.0;
			if (x0 != 0 && ε * ξ > 0)
			{
				t = Calculation.SolveEquation9_3(x0, ε, ξ, 1, L, 50, ResourceProvider.SimulationSettings.λ);
			}

			if (t < 0)
				t = x0 > ResourceProvider.SimulationSettings.λ ? 1 : 1000;

			foreach (var node in net)
			{
				var neighbours = node
					.GetConnections()
					.SelectMany(c => c.GetConnectedNodes()
						.Where(n => !n.Equals(node)));
				foreach (var neighbour in neighbours)
				{
					var nodeMetrics = neighbour.GetNodeState();

					if ((!ResourceProvider.SimulationSettings.UseOnlyIsActiveNodes || neighbour.IsActive()) && !neighbour.IsInfected())
					{
						var connection = node.GetConnections()
							.First(x => x.GetConnectedNodes().Any(x => x == neighbour));

						var queue = nodeMetrics.MessagesInQueueSize / nodeMetrics.Throughput + connection.TimeWaiting;
						if (queue < 1)
							queue = 1;
						var weight = queue;
						if (queue < t)
						{
							_graph.AddEdge(int.Parse(node.GetId()), int.Parse(neighbour.GetId()), weight);
						}
					}
				}
			}
		}
	}
}