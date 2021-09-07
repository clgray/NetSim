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
            ResourceProvider.CurrentTime = startTime;
            var stopSignal = false;

            InitNetwork(tag);

            var nodes = ResourceProvider.NodeProvider.GetNodes();
            var nodesIds = nodes.Select(x => x.GetId()).ToList();

            List<Message> messages= new List<Message>(); 

            for (int i = 0; !stopSignal; i++)
            {
	            messages.AddRange(GenerateMessages(nodesIds, ResourceProvider.CurrentTime, nodes));
	            ResourceProvider.CurrentTime = startTime.AddSeconds(i * _settings.TimeDelta);
                foreach (var node in nodes)
                {
                    var states = node.Send(ResourceProvider.CurrentTime);
                }
                ResourceProvider.MetricsLogger.WriteConnectionMetrics();
                ResourceProvider.MetricsLogger.WriteNodeMetrics();
                ResourceProvider.MetricsLogger.WriteMessageMetrics(messages);
                messages = messages.Where(x => x.State != MessageState.Delivered).ToList();

                if (ResourceProvider.MessagesUnDelivered == 0)
                {
                    stopSignal = true;
                }

            }

            //ResourceProvider.MetricsLogger.WriteMessageMetrics(messages);

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
            ResourceProvider.MessagesUnDelivered += messages.Count;
            return messages;
        }

        private void InitNetwork(string tag)
        {
            ResourceProvider.InitProviders(_settings, tag);
        }
    }
}
