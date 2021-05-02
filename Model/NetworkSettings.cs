using System;
using System.Collections.Generic;
using System.Text;
using NetSim.Model.Connection;
using NetSim.Model.Message;
using NetSim.Model.Node;

namespace NetSim.Model
{
    public class NetworkSettings
    {
        public List<NodeSettings> NodeSettings { get; set; }
        public List<ConnectionSettings> ConnectionSettings { get; set; }
        public MessagesSettings MessagesSettings { get; set; }
    }
}
