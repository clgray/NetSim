using NetSim.Model.Message;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NetSim.Model;

namespace NetSim.Lib
{
    public interface INode
    {
        public IEnumerable<State> Send();
        public State Receive(Message data);
        public string GetId();
        public List<IConnection> GetConnections();
        public void AddConnection(IConnection connection);
    }
}
