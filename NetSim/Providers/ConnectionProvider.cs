using System.Collections.Generic;
using System.Linq;
using NetSim.Lib;
using NetSim.Lib.Connections;
using NetSim.Model;

namespace NetSim.Providers
{
    public class ConnectionProvider
    {
        private readonly NetworkSettings _settings;
        private readonly List<IConnection> _connections;

        public ConnectionProvider(NetworkSettings settings)
        {
            _settings = settings;
            _connections = GenerateConnections();
        }

        public List<IConnection> GenerateConnections()
        {
            var connections = new List<IConnection>();

            foreach (var connectionSettings in _settings.ConnectionSettings)
            {
                var connectedNodes = connectionSettings.NodeIds.Select(nodeId => ResourceProvider.NodeProvider.GetNode(nodeId)).ToList();

                // TODO: Connection fabric
                var connection = new WiredConnection(connectionSettings, connectedNodes); 

                foreach (var node in connectedNodes)
                {
                    node.AddConnection(connection);
                }

                connections.Add(connection);
            }

            return connections;
        }

        public IConnection GetConnection(INode node)
        {
            return _connections.Find(x => x.IsConnected(node));
        }
    }
}
