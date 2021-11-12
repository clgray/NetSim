using System;
using System.Collections.Generic;
using System.Linq;
using NetSim.Model;
using NetSim.Model.Message;
using NetSim.Model.Node;
using NetSim.Providers;

namespace NetSim.Lib.Networking
{
    public class DefaultNetworking : INetworking
    {
        private readonly NetworkSettings _settings;
        private readonly IMessageGenerator _messageGenerator;
        private List<Message> _messagesTotal;
        private List<NodeMetrics> _nodeMetrics;

        public DefaultNetworking(NetworkSettings networkSettings, IMessageGenerator messageGenerator)
        {
            _messageGenerator = messageGenerator;
            _settings = networkSettings;
            _messagesTotal = new List<Message>();
            _nodeMetrics = new List<NodeMetrics>();
        }

        public void StartSimulation()
        {
            var tag = Guid.NewGuid().ToString();
            var startTime = DateTime.Parse("2021-06-01 04:00:00").ToUniversalTime();
            var currentTime = startTime.AddSeconds(0);
            var stopSignal = false;
            var currentFailedMessages = 0;

            InitNetwork(tag);

            var nodes = ResourceProvider.NodeProvider.GetNodes();
            var nodesIds = nodes.Select(x => x.GetId()).ToList();

            var messages = GenerateMessagesInit(nodesIds, currentTime, nodes);
            _messagesTotal.AddRange(messages);
            ResourceProvider.MessagesUnDelivered += messages.Count;

            for (var i = 0; !stopSignal; i++)
            {
                currentTime = startTime.AddSeconds(i * _settings.TimeDelta);

                ResourceProvider.RouterProvider.GetRouter(_settings.NodeSettings.First().RoutingAlgorithm)
                    .RebuildRoutes();
                foreach (var node in nodes)
                {
                    var states = node.Send(currentTime);
                   
                }

                var messagesFailedCurrentIteration = ResourceProvider.MessagesDeliverFailed - currentFailedMessages;
                currentFailedMessages = ResourceProvider.MessagesDeliverFailed;

                ResourceProvider.MetricsLogger.WriteFailedMessagesCount(messagesFailedCurrentIteration);
                ResourceProvider.MetricsLogger.WriteConnectionMetrics();
                ResourceProvider.MetricsLogger.WriteNodeMetrics();

                IterationEnd(currentTime, nodes);

                if (ResourceProvider.MessagesUnDelivered == 0)
                {
                    stopSignal = true;
                }

            }

            ResourceProvider.MetricsLogger.WriteMessageMetrics(_messagesTotal);

            Console.WriteLine(tag);
            var count = _messagesTotal.Count;
            var averageTime = _messagesTotal.Select(x => x.TimeSpent).Aggregate((x, y) => x + y) / count;
            var averageSize = _messagesTotal.Select(x => x.Size).Aggregate((x, y) => x + y) / count;
            Console.WriteLine($"Среднее время для доставки сообщения: {averageTime}");
            Console.WriteLine($"Сообщений всего: {_messagesTotal.Count}");
            Console.WriteLine($"Сообщений не доставлено: {ResourceProvider.MessagesDeliverFailed}");
            Console.WriteLine($"Средний размер сообщения: {averageSize}");

        }

        public void StopSimulation()
        {
            throw new NotImplementedException();
        }

        private List<Message> GenerateMessagesInit(List<string> nodesIds, DateTime currentTime, List<INode> nodes)
        {
            var messages = _messageGenerator.Init(_settings.MessagesSettings, nodesIds, currentTime);

            DistributeMessages(messages, nodes);

            return messages;
        }

        private List<Message> GenerateMessages(DateTime currentTime, List<INode> nodes)
        {
            var messages = _messageGenerator.GenerateMessages(currentTime);

            DistributeMessages(messages, nodes);

            return messages;
        }

        private void DistributeMessages(IEnumerable<Message> messages, List<INode> nodes)
        {
            foreach (var message in messages)
            {
                var node = nodes.Find(x => x.GetId().Equals(message.StartId));
                node!.Receive(message);
            }
        }

        private void IterationEnd(DateTime time, List<INode> nodes)
        {
            if (_messageGenerator.GenerateInProgress())
            {
                var messages = GenerateMessages(time, nodes);
                _messagesTotal.AddRange(messages);
                ResourceProvider.MessagesUnDelivered += messages.Count;
            }

            // TODO: Get nodes state and rebuild routes here
        }

        private void InitNetwork(string tag)
        {
            ResourceProvider.InitProviders(_settings, tag);
        }

        private void UpdateNodeStates()
        {
            _nodeMetrics = ResourceProvider.NodeProvider.GetNodes().Select(x => x.GetNodeState()).ToList();
        }
}
}
