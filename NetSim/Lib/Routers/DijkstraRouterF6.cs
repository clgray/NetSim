using System.Collections.Generic;
using System.Linq;
using NetSim.Lib.Routers.Percalation;

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
					.SelectMany(x => x.GetConnectedNodes()
						.Where(x => !x.Equals(node)));
				foreach (var neighbour in neighbours)
				{
					var nodeMetrics = neighbour.GetNodeState();
					var ε = nodeMetrics.MessagesReceivedSize / 100.0;
					var ξ = nodeMetrics.MessagesSentSize / 100.0;
					var x0 = nodeMetrics.MessagesTotalSize / 100.0;
					var L = nodeMetrics.Throughput /100.0;
					//var ε = nodeMetrics.MessagesReceived;
					//var ξ = nodeMetrics.MessagesSent;
					//var x0 = nodeMetrics.MessagesInQueue;
					//var L = 5;
					var weight = 0.001;
					//if (ε > ξ)
					{
						var t = Calculation.SolveEquation6(x0, ε, ξ, 1, L, 50);
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