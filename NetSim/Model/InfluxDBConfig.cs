using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSim.Model
{
    public class InfluxDBConfig
    {
        public string Hostname { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
