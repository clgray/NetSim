using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using NetSim.Model.Connection;
using NetSim.Model.Message;
using NetSim.Model.Node;
using NetSim.Providers;
using NetSim.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetSim.Lib
{
    public class InfluxDbMetricsLogger : IMetricsLogger
    {
        private readonly IDBProvider _provider;
        private readonly WriteApi _writeApi;
        private readonly WriteApiAsync _writeApiAsync;
        private List<Task> _tasks;

        public InfluxDbMetricsLogger(IDBProvider dbProvider)
        {
            _provider = dbProvider;
            _writeApi = dbProvider.GetWriteApi();
            _writeApiAsync = dbProvider.GetWriteApiAsync();
            _tasks = new List<Task>();
        }

        public void WriteMessageMetrics(IEnumerable<Message> messages)
        {
            var messageMetrics = messages.Select(x => new MessageMetrics()
            {
                State = MessageState.Received.ToString(),
                Size = x.Size,
                Path = string.Join('|', x.Path),
                TimeSpent = x.TimeSpent,
                Time = x.StartTime.AddSeconds(x.TimeSpent),
                data = x.Data,
                Tag = ResourceProvider.Tag
            }).ToList();

            //var writeApi = _provider.GetWriteApi();
            var task = _writeApiAsync.WriteMeasurementsAsync(WritePrecision.S, messageMetrics);
            _tasks.Add(task);
            Task.WaitAll(_tasks.ToArray());
        }

        public void WriteNodeMetrics()
        {
            return;
        }

        public void WriteConnectionMetrics()
        {
            return;
        }

        public void CollectNodeMetrics(NodeMetrics nodeMetrics)
        {
            _writeApi.WriteMeasurement(WritePrecision.S, nodeMetrics);
        }

        public void CollectConnectionMetrics(ConnectionMetrics connectionMetrics)
        {
            _writeApi.WriteMeasurement(WritePrecision.S, connectionMetrics);
        }
    }
}
