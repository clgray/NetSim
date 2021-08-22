using NetSim.Model.Connection;
using NetSim.Model.Message;
using NetSim.Model.Node;
using System.Collections.Generic;

namespace NetSim.Lib
{
    public interface IMetricsLogger
    {
        public void WriteMessageMetrics(IEnumerable<Message> messages);
        public void WriteNodeMetrics();
        public void WriteConnectionMetrics();
        public void CollectNodeMetrics(NodeMetrics connectionMetrics);
        public void CollectConnectionMetrics(ConnectionMetrics connectionMetrics);
    }
}
