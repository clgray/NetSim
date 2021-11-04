using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSim.Model.Message;
using NetSim.Providers;

namespace NetSim.Lib.Routers
{
    public class DijkstraRouter : IRouter
    {
        private List<DijkstraNode> _nodes;
        private DijkstraRouterHelper _helper;


        public DijkstraRouter()
        {
            _helper = new DijkstraRouterHelper();
        }

        public DijkstraRouter(IEnumerable<INode> nodes)
        {
            _helper = new DijkstraRouterHelper();
            _nodes = nodes.Select(x => new DijkstraNode(x)).ToList();
        }

        public INode GetRoute(INode currentNode, string targetId, Message message)
        {
            var node = GetRouteWithAvailability(currentNode, targetId, message);
            if (node is null)
            {
                return GetRouteDefault(currentNode, targetId, message);
            }
            else
            {
                // Saved path isn't relevant anymore
                _helper.DeletePathForMessage(message);
            }

            return node;
        }

        // TODO: need refactoring
        private INode GetRouteDefault(INode currentNode, string targetId, Message message)
        {
            if (_nodes == null)
            {
                SetNodes();
            }

            var start = _nodes.Find(x => x.Node.Equals(currentNode));
            var destination = _nodes.Find(x => x.Node.GetId().Equals(targetId));

            var nodeFromSavedPath = _helper.GetNextNodeFromSavedPath(message, start);
            if (nodeFromSavedPath != null)
            {
                return nodeFromSavedPath;
            }

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

            _helper.SavePath(message, shortestPath);

            // skipping start nodes
            return shortestPath.Skip(1).First().Node;
        }

        private INode GetRouteWithAvailability(INode currentNode, string targetId, Message message)
        {
            if (_nodes == null)
            {
                SetNodes();
            }

            SetVisitedNodes(message);

            var start = _nodes.Find(x => x.Node.Equals(currentNode));
            var destination = _nodes.Find(x => x.Node.GetId().Equals(targetId));

            start.IsVisited = false;

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
					// Check if connection are overloaded
					if (cnn.GetLoad() > 0.9)
					{
						continue;
					}

					var childNode = cnn.GetConnectedNodes().ToList().Find(x => !x.Equals(node.Node));
                    var childDijkstraNode = _nodes.Find(x => x.Node.Equals(childNode));

                    if (childDijkstraNode!.IsVisited)
                        continue;

                    // Check if Node are overloaded
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

        private void SetVisitedNodes(Message message)
        {
            foreach (var nodeId in message.Path)
            {
                var node = _nodes.Find(x => x.Node.GetId().Equals(nodeId));
                node.IsVisited = true;
            }
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
