using System.Collections.Generic;
using System.Linq;

namespace NetSim.Lib.Routers
{
	public class DijkstraRouterByLoads : DijkstraRouterBase
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
					var weight = 1;

                    var nodeMetrics = neighbour.GetNodeState();

					if (neighbour.Load() > 0.9)
					//if (!neighbour.IsAvailable())
					weight = 10000;

					var connection = node.GetConnections().First(x => x.GetConnectedNodes().Any(x => x == neighbour));

					if (connection.GetLoad() > 0.9)
						weight = 10000;

					_graph.AddEdge(int.Parse(node.GetId()), int.Parse(neighbour.GetId()), weight);
				}
			}
		}
	}
}