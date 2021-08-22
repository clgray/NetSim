using System;
using System.Collections.Generic;
using System.Linq;
using NetSim.Model;
using NetSim.Model.Message;
using NetSim.Providers;

namespace NetSim.Lib.Networking
{
    public class DefaultNetworking : INetworking
    {
        private readonly NetworkSettings _settings;
        private readonly IMessageGenerator _messageGenerator;

        public DefaultNetworking(NetworkSettings networkSettings, IMessageGenerator messageGenerator)
        {
            _messageGenerator = messageGenerator;
            _settings = networkSettings;
        }

        public void StartSimulation()
        {
            var tag = Guid.NewGuid().ToString();
            var startTime = DateTime.Parse("2021-06-01 04:00:00").ToUniversalTime();
            var currentTime = startTime.AddSeconds(0);
            var stopSignal = false;

            InitNetwork(tag);

            var nodes = ResourceProvider.NodeProvider.GetNodes();
            var nodesIds = nodes.Select(x => x.GetId()).ToList();

            var messages = GenerateMessages(nodesIds, currentTime, nodes);

            for (int i = 0; !stopSignal; i++)
            {
                currentTime = startTime.AddSeconds(i * _settings.TimeDelta);

                foreach (var node in nodes)
                {
                    var states = node.Send(currentTime);
                }
                ResourceProvider.MetricsLogger.WriteConnectionMetrics();
                ResourceProvider.MetricsLogger.WriteNodeMetrics();

                if (ResourceProvider.MessagesUnDelivered == 0)
                {
                    stopSignal = true;
                }

            }

            ResourceProvider.MetricsLogger.WriteMessageMetrics(messages);

            Console.WriteLine(tag);
            var count = messages.Count;
            var averageTime = messages.Select(x => x.TimeSpent).Aggregate((x, y) => x + y) / count;
            var averageSize = messages.Select(x => x.Size).Aggregate((x, y) => x + y) / count;
            Console.WriteLine($"Среднее время для доставки сообщения: {averageTime}");
            Console.WriteLine($"Средний размер сообщения: {averageSize}");

        }

        public void StopSimulation()
        {
            throw new NotImplementedException();
        }

        private List<Message> GenerateMessages(List<string> nodesIds, DateTime currentTime, List<INode> nodes)
        {
            var messages = _messageGenerator.GenerateMessages(_settings.MessagesSettings, nodesIds, currentTime);

            foreach (var message in messages)
            {
                var node = nodes.Find(x => x.GetId().Equals(message.StartId));
                node!.Receive(message);
            }

            return messages;
        }

        private void InitNetwork(string tag)
        {
            ResourceProvider.InitProviders(_settings, tag);
        }
    }
}
