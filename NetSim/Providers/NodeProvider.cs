using System.Collections.Generic;
using System.Linq;
using NetSim.Lib;
using NetSim.Lib.Nodes;
using NetSim.Model;
using NetSim.Model.Node;

namespace NetSim.Providers
{
    public class NodeProvider
    {
        private readonly NetworkSettings _settings;
        private readonly List<INode> _nodes;

        public NodeProvider(NetworkSettings settings)
        {
            _settings = settings;
            _nodes = CreateNodes();
        }

        public INode GetNode(string id)
        {
            return _nodes.Find(x => x.GetId().Equals(id));
        }
        public List<INode> GetNodes()
        {
            return _nodes;
        }

        private List<INode> CreateNodes()
        {
            return _settings.NodeSettings.Select(x=> CreateNode(x, _settings.SimulationSettings.RoutingAlgorithm)).ToList();
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private INode CreateNode(NodeSettings nodeSettings, string routingAlgorithm)
        {
            return new IpNode(nodeSettings, _settings.TimeDelta, routingAlgorithm);
        }
    }
}
