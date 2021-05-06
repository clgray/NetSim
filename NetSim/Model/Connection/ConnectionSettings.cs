using System;
using System.Collections.Generic;
using System.Text;
using NetSim.Lib;

namespace NetSim.Model.Connection
{
    public class ConnectionSettings
    {
        public int Bandwidth { get; set; }
        public int TimeUntilShutdown { get; set; }
        public List<string> NodeIds { get; set; }
    }
}
