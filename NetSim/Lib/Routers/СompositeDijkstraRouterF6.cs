using System.Collections.Generic;
using System.Linq;
using NetSim.Lib.Routers.Percolation;
using NetSim.Providers;

namespace NetSim.Lib.Routers
{
	public class СompositeDijkstraRouterF6 : DijkstraRouterBase
	{
		protected override void SetNodes(IReadOnlyCollection<INode> net)
		{
			_graph = new Graph(net.Count);

			foreach (var node in net)
			{
				var neighbours = node
					.GetConnections()
					.SelectMany(c => c.GetConnectedNodes()
						.Where(n => !n.Equals(node)));
				foreach (var neighbour in neighbours)
				{
					var nodeMetrics = neighbour.GetNodeState();
					var ε = nodeMetrics.MessagesReceivedSize ;
					var ξ = nodeMetrics.Throughput;
					var x0 = nodeMetrics.MessagesInQueueSize ;
					var L = nodeMetrics.Throughput * ResourceProvider.SimulationSettings.MultiplierThresholdToBlock;

					if (!ResourceProvider.SimulationSettings.UseOnlyIsActiveNodes || neighbour.IsActive())
					{
						var weight = 0.01;
						var connection = node.GetConnections().First(x => x.GetConnectedNodes().Any(x => x == neighbour));

						var queue = nodeMetrics.MessagesInQueueSize / nodeMetrics.Throughput + connection.TimeWaiting;
						if (queue < 1)
							queue = 1;
						
						if (x0 > L / 2)
						{
							var t = Calculation.SolveEquation6(x0, ε, ξ, 1, L, 50, ResourceProvider.SimulationSettings.λ);
							if (t < 0)
								t = x0 > L ? 1 : 100;
							weight = 1 / t;
						}

						weight = (weight * queue);
						//if (weight < 1)
						//	weight = 1;

						_graph.AddEdge(int.Parse(node.GetId()), int.Parse(neighbour.GetId()), weight);
					}
				}
			}
		}
	}
}