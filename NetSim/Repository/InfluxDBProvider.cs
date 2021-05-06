using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using InfluxDB.Collector;
using NetSim.Model;

namespace NetSim.Repository
{
    // ReSharper disable once InconsistentNaming
    public class InfluxDBProvider : IDBProvider
    {
        private readonly InfluxDBConfig _config;
        public InfluxDBProvider()
        {
            _config = ReadConfig();
        }
        public MetricsCollector GetMetricsCollector(string tag, string type)
        {
            var collector = new CollectorConfiguration()
                .Tag.With(tag, type)
                .Batch.AtInterval(TimeSpan.FromSeconds(1))
                .WriteTo.InfluxDB(_config.Hostname, _config.Database, _config.User, _config.Password)
                .CreateCollector();
            
            return collector;
        }

        private static InfluxDBConfig ReadConfig()
        {
            var json = File.ReadAllText(Environment.CurrentDirectory + "appsettings.json");
            return JsonSerializer.Deserialize<Settings>(json)?.InfluxDBConfig;
        }
    }
}
