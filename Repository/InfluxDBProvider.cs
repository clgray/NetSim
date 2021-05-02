using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using InfluxDB.Collector;
using NetSim.Model;

namespace NetSim.Repository
{
    public class InfluxDBProvider : IDBProvider
    {
        private readonly InfluxDBConfig _config;
        public InfluxDBProvider()
        {
            _config = ReadConfig();
        }
        public void CreateMetricsCollector(string tag, string type)
        {
            Metrics.Collector = new CollectorConfiguration()
                .Tag.With(tag, type)
                .Batch.AtInterval(TimeSpan.FromSeconds(1))
                .WriteTo.InfluxDB(_config.Hostname, _config.Database, _config.User, _config.Password)
                .CreateCollector();
            throw new NotImplementedException();
        }

        private InfluxDBConfig ReadConfig()
        {
            var json = File.ReadAllText(Environment.CurrentDirectory + "appsettings.json");
            return JsonSerializer.Deserialize<Settings>(json)?.InfluxDBConfig;
        }
    }
}
