using System;
using System.Collections.Generic;
using NetSim.Model.Connection;

namespace NetSim.Model.Node
{
    public class NodeSettings
    {
        public string Id { get; set; }
        [Obsolete]
        public string RoutingAlgorithm { get; set; }
        public int Throughput { get; set; }
        //public List<ConnectionSettings> Connections { get; set; }
    }
}
