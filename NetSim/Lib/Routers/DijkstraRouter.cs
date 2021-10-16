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

        public DijkstraRouter()
        {
            
        }

        public DijkstraRouter(IEnumerable<INode> nodes)
        {
            _nodes = nodes.Select(x => new DijkstraNode(x)).ToList();
        }

        public INode GetRoute(INode currentNode, string targetId)
        {
            var node = GetRouteWithAvailability(currentNode, targetId);
            if (node is null)
            {
                return GetRouteDefault(currentNode, targetId);
            }

            return node;
        }

        // TODO: need refactoring
        private INode GetRouteDefault(INode currentNode, string targetId)
        {
            if (_nodes == null)
            {
                SetNodes();
            }

            var start = _nodes.Find(x => x.Node.Equals(currentNode));
            var destination = _nodes.Find(x => x.Node.GetId().Equals(targetId));

            DijkstraSearch(start, destination);
            var shortestPath = new List<DijkstraNode> { destination };
            BuildShortestPath(shortestPath, destination);

            foreach (var dijkstraNode in _nodes)
            {
                dijkstraNode.IsVisited = false;
                dijkstraNode.MaxBandwidthToStart = -1f;
                dijkstraNode.NearestToStart = null;
            }

            shortestPath.Reverse();

            if (shortestPath.Count() <= 1)
            {
                return null;
            }

            // skipping start nodes
            return shortestPath.Skip(1).First().Node;
        }

        private INode GetRouteWithAvailability(INode currentNode, string targetId)
        {
            if (_nodes == null)
            {
                SetNodes();
            }

            var start = _nodes.Find(x => x.Node.Equals(currentNode));
            var destination = _nodes.Find(x => x.Node.GetId().Equals(targetId));

            DijkstraSearchWithAvailability(start, destination);
            var shortestPath = new List<DijkstraNode> { destination };
            BuildShortestPath(shortestPath, destination);

            foreach (var dijkstraNode in _nodes)
            {
                dijkstraNode.IsVisited = false;
                dijkstraNode.MaxBandwidthToStart = -1f;
                dijkstraNode.NearestToStart = null;
            }

            shortestPath.Reverse();

            if (shortestPath.Count() <= 1)
            {
                return null;
            }

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
            var prioQueue = new List<DijkstraNode> {start};

            do
            {
                prioQueue = prioQueue.OrderBy(x => x.MaxBandwidthToStart).ToList();
                var node = prioQueue.First();
                prioQueue.Remove(node);


                foreach (var cnn in node.Node.GetConnections().OrderBy(x => 1/x.GetBandwidth()))
                {
                    var childNode = cnn.GetConnectedNodes().ToList().Find(x => !x.Equals(node.Node));
                    var childDijkstraNode = _nodes.Find(x => x.Node.Equals(childNode));

                    if (childDijkstraNode!.IsVisited)
                        continue;

                    if (childDijkstraNode!.MaxBandwidthToStart == -1f
                        ||
                        node.MaxBandwidthToStart + 1 / cnn.GetBandwidth() < childDijkstraNode.MaxBandwidthToStart)
                    {

                        if (childDijkstraNode!.MaxBandwidthToStart == -1f)
                        {
                            childDijkstraNode!.MaxBandwidthToStart = 0;
                        }

                        childDijkstraNode.MaxBandwidthToStart = node.MaxBandwidthToStart + 1 / cnn.GetBandwidth();
                        childDijkstraNode.NearestToStart = node;

                        if (!prioQueue.Contains(childDijkstraNode))
                        {
                            prioQueue.Add(childDijkstraNode);
                        }
                    }
                }

                node.IsVisited = true;
                if (node == end)
                {
                    return;
                }
            } while (prioQueue.Any());
        }

        private void DijkstraSearchWithAvailability(DijkstraNode start, DijkstraNode end)
        {
            start.MaxBandwidthToStart = 0;
            var prioQueue = new List<DijkstraNode> { start };

            do
            {
                prioQueue = prioQueue.OrderBy(x => x.MaxBandwidthToStart).ToList();
                var node = prioQueue.First();
                prioQueue.Remove(node);


                foreach (var cnn in node.Node.GetConnections().OrderBy(x => 1 / x.GetBandwidth()))
                {
                    var childNode = cnn.GetConnectedNodes().ToList().Find(x => !x.Equals(node.Node));
                    var childDijkstraNode = _nodes.Find(x => x.Node.Equals(childNode));

                    if (childDijkstraNode!.IsVisited)
                        continue;

                    if (!childDijkstraNode.Node.IsAvailable())
                    {
                        continue;
                    }

                    if (childDijkstraNode!.MaxBandwidthToStart == -1f
                        ||
                        node.MaxBandwidthToStart + 1 / cnn.GetBandwidth() < childDijkstraNode.MaxBandwidthToStart)
                    {

                        if (childDijkstraNode!.MaxBandwidthToStart == -1f)
                        {
                            childDijkstraNode!.MaxBandwidthToStart = 0;
                        }

                        childDijkstraNode.MaxBandwidthToStart = node.MaxBandwidthToStart + 1 / cnn.GetBandwidth();
                        childDijkstraNode.NearestToStart = node;

                        if (!prioQueue.Contains(childDijkstraNode))
                        {
                            prioQueue.Add(childDijkstraNode);
                        }
                    }
                }

                node.IsVisited = true;
                if (node == end)
                {
                    return;
                }
            } while (prioQueue.Any());
        }
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
