using System;
using System.Collections.Generic;
using System.Text;
using NetSim.Providers;

namespace NetSim.Lib.Routers
{
    public class DijkstraRouter : IRouter
    {
        private readonly List<INode> _nodes;
        private readonly List<List<float>> _graph;

        public DijkstraRouter(List<INode> nodes)
        {
            _graph = GenerateGraphMatrix(nodes);
        }

        public INode GetRoute(INode currentNode, string targetId)
        {
            throw new NotImplementedException();
        }

        private List<List<float>> GenerateGraphMatrix(IReadOnlyList<INode> nodes)
        {
            var graph = new List<List<float>>();

            for (int i = 0; i < nodes.Count; i++)
            {
                var row = new List<float>();
                var connections = nodes[i].GetConnections();
                for (int j = 0; j < nodes.Count; j++)
                {
                    var value = 0f;
                    var node = nodes[j];

                    if (i == j)
                    {
                        value = 1f;
                    }

                    var connection = connections.Find(x => x.IsConnected(node));
                    if (connection != null)
                    {
                        value = connection.GetBandwidth();
                    }

                    row.Add(value);
                }
                graph.Add(row);
            }
            // TODO: check this shit
            return graph;
        }
    }
}
