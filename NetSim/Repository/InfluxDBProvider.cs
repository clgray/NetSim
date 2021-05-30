using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using InfluxDB.Client;
using InfluxDB.Collector;
using NetSim.Model;

namespace NetSim.Repository
{
    // ReSharper disable once InconsistentNaming
    public class InfluxDBProvider : IDBProvider
    {
        private readonly InfluxDBConfig _config;
        private readonly InfluxDBClient _client;

        public InfluxDBProvider()
        {
            _config = ReadConfig();
            var options = InfluxDBClientOptions.Builder.CreateNew()
                .Url(_config.Hostname)
                .Bucket(_config.Database)
                .Authenticate(_config.User, _config.Password.ToCharArray())
                .Build();

            _client = InfluxDBClientFactory.Create(options);
        }
        public InfluxDBProvider(string tag)
        {
            _config = ReadConfig();
            var options = InfluxDBClientOptions.Builder.CreateNew()
                .Url(_config.Hostname)
                .Bucket(_config.Database)
                .AddDefaultTag(tag, "")
                .Org("self")
                .Authenticate(_config.User, _config.Password.ToCharArray())
                .Build();

            _client = InfluxDBClientFactory.Create(options);
        }

        public WriteApi GetWriteApi()
        {
            return _client.GetWriteApi();
        }
        public WriteApiAsync GetWriteApiAsync()
        {
            return _client.GetWriteApiAsync();
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
            var json = File.ReadAllText(Environment.CurrentDirectory + "\\appsettings.json");
            return JsonSerializer.Deserialize<Settings>(json)?.InfluxDBConfig;
        }
    }
}
