using System;
using System.Collections.Generic;
using System.Text;
using InfluxDB.Collector;

namespace NetSim.Repository
{
    public interface IDBProvider
    {
        public MetricsCollector GetMetricsCollector();

    }
}
