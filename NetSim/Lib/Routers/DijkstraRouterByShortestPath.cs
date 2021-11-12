using System.Collections.Generic;
using System.Linq;

namespace NetSim.Lib.Routers
{
	public class DijkstraRouterByShortestPath : DijkstraRouterBase
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
					var weight = 1;

					_graph.AddEdge(int.Parse(node.GetId()), int.Parse(neighbour.GetId()), weight);
				}
			}
		}
	}
}