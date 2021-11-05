using NetSim.Model.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using NetSim.Providers;
using static System.Linq.Enumerable;
using EdgeList = System.Collections.Generic.List<(int node, double weight)>;

namespace NetSim.Lib.Routers
{
	public class Dijkstra : IRouter
	{
		private Graph _graph;

		public Dijkstra(INode[] net)
		{
			SetNodes(net);
		}

		public Dijkstra()
		{
		}


		public int[] FindPath(int startNode, int endNode)
		{
			var path = _graph.FindPath(startNode);
			if (double.IsPositiveInfinity(path[endNode].distance)) return new int[0];
			return Path(path, startNode, endNode).Select(p => p.node).ToArray();
		}

		public IEnumerable<(int destination, int[] path)> FindPaths(INode startNode)
		{
			var id = int.Parse(startNode.GetId());
			var path = _graph.FindPath(id);
			for (int i = 0; i < path.Length; i++)
			{
				yield return (i, FindPath(id, i));
			}
		}

		public INode GetRoute(INode currentNode, string targetId, Message message)
		{
			if (_graph == null)
			{
				SetNodes();
			}

			var path = FindPath(int.Parse(currentNode.GetId()), int.Parse(targetId));
			if (path == null || path.Length == 0)
				return null;

			var next = path[^2].ToString();
			return currentNode.GetConnections().SelectMany(x => x.GetConnectedNodes()).First(x => x.GetId() == next);
		}

		public void RebuildRoutes()
		{
			SetNodes();
		}

		private void SetNodes()
		{
			var net = ResourceProvider.NodeProvider.GetNodes();
			SetNodes(net);
		}

		private void SetNodes(IReadOnlyCollection<INode> net)
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

					if (!neighbour.IsAvailable())
						weight = 10000;

					var connection = node.GetConnections().First(x => x.GetConnectedNodes().Any(x => x == neighbour));

					if (connection.GetLoad() > 0.9)
						weight = 10000;

					_graph.AddEdge(int.Parse(node.GetId()), int.Parse(neighbour.GetId()), weight);
				}
			}
		}


		IEnumerable<(double distance, int node)> Path((double distance, int prev)[] path, int start, int destination)
		{
			yield return (path[destination].distance, destination);
			for (int i = destination; i != start; i = path[i].prev)
			{
				yield return (path[path[i].prev].distance, path[i].prev);
			}
		}

		sealed class Graph
		{
			private readonly List<EdgeList> adjacency;

			public Graph(int vertexCount) => adjacency = Range(0, vertexCount).Select(v => new EdgeList()).ToList();

			public int Count => adjacency.Count;
			public bool HasEdge(int s, int e) => adjacency[s].Any(p => p.node == e);
			public bool RemoveEdge(int s, int e) => adjacency[s].RemoveAll(p => p.node == e) > 0;

			public bool AddEdge(int s, int e, double weight)
			{
				if (HasEdge(s, e)) return false;
				adjacency[s].Add((e, weight));
				return true;
			}

			public (double distance, int prev)[] FindPath(int start)
			{
				var info = Range(0, adjacency.Count).Select(i => (distance: double.PositiveInfinity, prev: i))
					.ToArray();
				info[start].distance = 0;
				var visited = new System.Collections.BitArray(adjacency.Count);

				var heap = new Heap<(int node, double distance)>((a, b) => a.distance.CompareTo(b.distance));
				heap.Push((start, 0));
				while (heap.Count > 0)
				{
					var current = heap.Pop();
					if (visited[current.node]) continue;
					var edges = adjacency[current.node];
					for (int n = 0; n < edges.Count; n++)
					{
						int v = edges[n].node;
						if (visited[v]) continue;
						double alt = info[current.node].distance + edges[n].weight;
						if (alt < info[v].distance)
						{
							info[v] = (alt, current.node);
							heap.Push((v, alt));
						}
					}

					visited[current.node] = true;
				}

				return info;
			}
		}

		sealed class Heap<T>
		{
			private readonly IComparer<T> comparer;
			private readonly List<T> list = new List<T> {default};

			public Heap() : this(default(IComparer<T>))
			{
			}

			public Heap(IComparer<T> comparer)
			{
				this.comparer = comparer ?? Comparer<T>.Default;
			}

			public Heap(Comparison<T> comparison) : this(Comparer<T>.Create(comparison))
			{
			}

			public int Count => list.Count - 1;

			public void Push(T element)
			{
				list.Add(element);
				SiftUp(list.Count - 1);
			}

			public T Pop()
			{
				T result = list[1];
				list[1] = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
				SiftDown(1);
				return result;
			}

			private static int Parent(int i) => i / 2;
			private static int Left(int i) => i * 2;
			private static int Right(int i) => i * 2 + 1;

			private void SiftUp(int i)
			{
				while (i > 1)
				{
					int parent = Parent(i);
					if (comparer.Compare(list[i], list[parent]) > 0) return;
					(list[parent], list[i]) = (list[i], list[parent]);
					i = parent;
				}
			}

			private void SiftDown(int i)
			{
				for (int left = Left(i); left < list.Count; left = Left(i))
				{
					int smallest = comparer.Compare(list[left], list[i]) <= 0 ? left : i;
					int right = Right(i);
					if (right < list.Count && comparer.Compare(list[right], list[smallest]) <= 0) smallest = right;
					if (smallest == i) return;
					(list[i], list[smallest]) = (list[smallest], list[i]);
					i = smallest;
				}
			}
		}
	}
}