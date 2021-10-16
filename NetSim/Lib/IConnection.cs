using System;
using System.Collections.Generic;
using System.Text;
using NetSim.Model;
using NetSim.Model.Message;

namespace NetSim.Lib
{
    public interface IConnection
    {
        public bool Send(Message data, INode receiver);
        public bool IsConnected(INode node);
        public IEnumerable<INode> GetConnectedNodes();
        public float GetBandwidth();
        public float GetLoad();
        public void ProgressQueue(DateTime currentTime);
        public void Enable();
        public void Disable();
    }
}
