using System;
using System.Collections.Generic;
using System.Linq;

namespace NetSim.Lib.Routers
{
	public class DijkstraRouterWithBlockedNodes : DijkstraRouterBase
	{
		protected override void SetNodes(IReadOnlyCollection<INode> net)
		{
			_graph = new Graph(net.Count);
			Random rnd = new Random();
			var blocked = rnd.Next(0, net.Count * 10);
			foreach (var node in net)
			{
				var neighbours = node
					.GetConnections()
					.SelectMany(x => x.GetConnectedNodes()
						.Where(x => !x.Equals(node)));
				foreach (var neighbour in neighbours)
				{
					if (int.Parse(neighbour.GetId()) == blocked) { neighbour.Disable();}
					var weight = 1;

					if (neighbour.IsActive())
					{
						_graph.AddEdge(int.Parse(node.GetId()), int.Parse(neighbour.GetId()), weight);
					}
				}
			}
		}
	}
}