using NetSim.Model.Message;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NetSim.Model;
using NetSim.Model.Node;

namespace NetSim.Lib
{
    public interface INode
    {
        public IEnumerable<State> Send(DateTime currentTime);
        public State Receive(Message data);
        public string GetId();
        public List<IConnection> GetConnections();
        public void AddConnection(IConnection connection);
        public bool IsAvailable();
        public float Load();
        public NodeMetrics GetNodeState();
    }
}
