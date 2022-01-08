using System.Collections.Generic;
using System.Linq;
using NetSim.Lib.Routers.Percalation;
using NetSim.Providers;

namespace NetSim.Lib.Routers
{
	public class DijkstraRouterF6 : DijkstraRouterBase
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
					//var ε = nodeMetrics.MessagesReceived;
					//var ξ = nodeMetrics.MessagesSent;
					//var x0 = nodeMetrics.MessagesInQueue;
					//var L = 5;
					if (!ResourceProvider.SimulationSettings.UseOnlyIsActiveNodes || neighbour.IsActive())
					{
						var weight = 0.001;
						if (x0 > L / 2)
						{
							var t = Calculation.SolveEquation6_q(x0, ε, ξ, 1, L, 50);
							if (t < 0)
								weight = x0 < L ? 0.001 : 1000;
							else
								weight = 1 / t;
						}

						_graph.AddEdge(int.Parse(node.GetId()), int.Parse(neighbour.GetId()), weight);
					}
				}
			}
		}
	}
}