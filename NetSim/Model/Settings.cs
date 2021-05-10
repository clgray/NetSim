using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSim.Model
{
    public class Settings
    {
        public InfluxDBConfig InfluxDBConfig { get; set; }
        public NetworkSettings NetworkSettings { get; set; }
    }
}
