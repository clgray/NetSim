using System;
using System.Collections.Generic;
using System.Text;
using InfluxDB.Client;
using InfluxDB.Collector;

namespace NetSim.Repository
{
    // ReSharper disable once InconsistentNaming
    public interface IDBProvider
    {
        public MetricsCollector GetMetricsCollector(string tag, string type);
        public WriteApiAsync GetWriteApi();


    }
}
