using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSim.Providers;

namespace NetSim.Lib.Routers
{
    public class DijkstraRouter : IRouter
    {
        private List<DijkstraNode> _nodes;
        //private readonly List<List<float>> _graph;

        public DijkstraRouter()
        {
            
        }

        public DijkstraRouter(List<INode> nodes)
        {
            _nodes = nodes.Select(x => new DijkstraNode(x)).ToList();
            //_graph = GenerateGraphMatrix(nodes);
        }

        public INode GetRoute(INode currentNode, string targetId)
        {
            if (_nodes == null)
            {
                SetNodes();
            }

            var start = _nodes.Find(x => x.Node.Equals(currentNode));
            var destination = _nodes.Find(x => x.Node.GetId().Equals(targetId));

            DijkstraSearch(start, destination);
            var shortestPath = new List<DijkstraNode> {destination};
            BuildShortestPath(shortestPath, destination);

            foreach (var dijkstraNode in _nodes)
            {
                dijkstraNode.IsVisited = false;
                dijkstraNode.MaxBandwidthToStart = -1f;
                dijkstraNode.NearestToStart = null;
            }

            shortestPath.Reverse();

            // skipping start nodes
            return shortestPath.Skip(1).First().Node;
        }

        private void SetNodes()
        {
            _nodes = ResourceProvider.NodeProvider.GetNodes().Select(x => new DijkstraNode(x)).ToList();
        }


        private void BuildShortestPath(List<DijkstraNode> list, DijkstraNode pathNode)
        {
            while (true)
            {
                if (pathNode.NearestToStart == null) 
                    return;
                list.Add(pathNode.NearestToStart);
                pathNode = pathNode.NearestToStart;
            }
        }


        private void DijkstraSearch(DijkstraNode start, DijkstraNode end)
        {
            start.MaxBandwidthToStart = 0;
            var prioQueue = new List<DijkstraNode>();
            prioQueue.Add(start);
            do
            {
                prioQueue = prioQueue.OrderByDescending(x => x.MaxBandwidthToStart).ToList();
                var node = prioQueue.First();
                prioQueue.Remove(node);
                foreach (var cnn in node.Node.GetConnections().OrderByDescending(x => x.GetBandwidth()))
                {
                    var childNode = cnn.GetConnectedNodes().ToList().Find(x => !x.Equals(node.Node));
                    var childDijkstraNode = _nodes.Find(x => x.Node.Equals(childNode));
                    if (childDijkstraNode!.IsVisited)
                        continue;
                    if (childDijkstraNode!.MaxBandwidthToStart == -1f 
                        ||
                        node.MaxBandwidthToStart + cnn.GetBandwidth() > childDijkstraNode.MaxBandwidthToStart)
                    {
                        childDijkstraNode.MaxBandwidthToStart = node.MaxBandwidthToStart + cnn.GetBandwidth();
                        childDijkstraNode.NearestToStart = node;
                        if (!prioQueue.Contains(childDijkstraNode))
                            prioQueue.Add(childDijkstraNode);
                    }
                }
                node.IsVisited = true;
                if (node == end)
                    return;
            } while (prioQueue.Any());
        }

        //private List<List<float>> GenerateGraphMatrix(IReadOnlyList<INode> nodes)
        //{
        //    var graph = new List<List<float>>();

        //    for (int i = 0; i < nodes.Count; i++)
        //    {
        //        var row = new List<float>();
        //        var connections = nodes[i].GetConnections();
        //        for (int j = 0; j < nodes.Count; j++)
        //        {
        //            var value = 0f;
        //            var node = nodes[j];

        //            if (i == j)
        //            {
        //                value = 1f;
        //            }

        //            var connection = connections.Find(x => x.IsConnected(node));
        //            if (connection != null)
        //            {
        //                value = connection.GetBandwidth();
        //            }

        //            row.Add(value);
        //        }
        //        graph.Add(row);
        //    }
        //    // TODO: check this shit
        //    return graph;
        //}
    }

    internal class DijkstraNode
    {
        public INode Node { get; set; }
        public DijkstraNode NearestToStart { get; set; }
        public bool IsVisited { get; set; } = false;
        public float MaxBandwidthToStart { get; set; } = -1f; 
        public DijkstraNode(INode node)
        {
            Node = node;
        }
    }
}
