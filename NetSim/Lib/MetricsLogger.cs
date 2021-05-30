using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Collector;
using NetSim.Model.Connection;
using NetSim.Model.Message;
using NetSim.Model.Node;
using NetSim.Providers;
using NetSim.Repository;

namespace NetSim.Lib
{
    public class MetricsLogger
    {
        private readonly IDBProvider _provider;
        private readonly WriteApiAsync _writeApi;
        private List<Task> _tasks;

        public MetricsLogger(IDBProvider dbProvider)
        {
            _provider = dbProvider;
            _writeApi = dbProvider.GetWriteApi();
            _tasks = new List<Task>();
        }

        public void WriteMessageMetrics(IEnumerable<Message> messages)
        {
            var messageMetrics = messages.Select(x => new MessageMetrics()
            {
                Received = (x.State == MessageState.Received),
                Size = x.Size,
                Path = string.Join('|', x.Path),
                TimeSpent = x.TimeSpent,
                Time = x.Time.AddSeconds(x.TimeSpent),
                data = x.Data,
                Tag = ResourceProvider.Tag

            }).ToList();

            //var writeApi = _provider.GetWriteApi();
            var task = _writeApi.WriteMeasurementsAsync(WritePrecision.S, messageMetrics);
            _tasks.Add(task);
        }

        public void WriteNodeMetrics(NodeMetrics nodeMetrics)
        {
            //var writeApi = _provider.GetWriteApi();
            var task = _writeApi.WriteMeasurementAsync(WritePrecision.S, nodeMetrics);
            _tasks.Add(task);
        }

        public void WriteConnectionMetrics(ConnectionMetrics connectionMetrics)
        {
            //var writeApi = _provider.GetWriteApi();
            _writeApi.WriteMeasurementAsync(WritePrecision.S, connectionMetrics).Wait();
            Task.WaitAll(_tasks.ToArray());
        }
    }
}
