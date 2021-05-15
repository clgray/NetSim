using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using NetSim.Providers;

namespace NetSim.Lib.Routers
{
    public class DFSRouter : IRouter
    {
        private List<DFSNode> _nodes;

        public DFSRouter()
        {
        
        }

        public INode GetRoute(INode currentNode, string targetId)
        {
            if (_nodes == null)
            {
                SetNodes();
            }

            var node = _nodes.Find(x => x.Node.Equals(currentNode));
            var path = Search(node, targetId);

            ResetState();

            if (path == null)
            {
                return null;
            }

            return path[1];
        }

        private List<INode> Search(DFSNode node, string targetId)
        {
            if (node.IsVisited)
            {
                return null;
            }

            node.IsVisited = true;

            var path = new List<INode>() { node.Node };

            if (node.Node.GetId().Equals(targetId))
            {
                return path;
            }

            foreach (var connection in node.Node.GetConnections())
            {
                var connectedNode = connection.GetConnectedNodes().ToList().Find(x => !x.Equals(node.Node));
                var nextNode = _nodes.Find(x => x.Node.Equals(connectedNode));

                var result = Search(nextNode, targetId);
                if (result != null)
                {
                    path.AddRange(result);
                    return path;
                }
            }

            return null;
        }

        private void ResetState()
        {
            foreach (var dfsNode in _nodes)
            {
                dfsNode.IsVisited = false;
            }
        }


        private void SetNodes()
        {
            _nodes = ResourceProvider.NodeProvider.GetNodes().Select(x => new DFSNode(x)).ToList();
        }
    }

    internal class DFSNode
    {
        public INode Node { get; set; }
        public bool IsVisited { get; set; }

        public DFSNode(INode node)
        {
            Node = node;
        }
    }
}
