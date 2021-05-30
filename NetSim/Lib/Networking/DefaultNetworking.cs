using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using NetSim.Model;
using System.Text.Json;
using NetSim.Model.Message;
using NetSim.Providers;

namespace NetSim.Lib.Networking
{
    public class DefaultNetworking : INetworking
    {
        private readonly NetworkSettings _settings;
        private readonly string _configPath;
        private readonly IMessageGenerator _messageGenerator;

        public DefaultNetworking(string configPath, IMessageGenerator messageGenerator)
        {
            _configPath = configPath;
            _messageGenerator = messageGenerator;
            _settings = ReadNetworkSettings();
        }

        public void StartSimulation()
        {
            var tag = Guid.NewGuid().ToString();
            var startTime = DateTime.Parse("2021-05-01 04:00:00").ToUniversalTime();
            var currentTime = startTime.AddSeconds(0); 
            var stopSignal = false;

            InitNetwork(tag);

            var nodes = ResourceProvider.NodeProvider.GetNodes();
            var nodesIds = nodes.Select(x => x.GetId()).ToList();

            var messages = GenerateMessages(nodesIds, currentTime, nodes);

            for (int i = 0; !stopSignal; i++)
            {
                currentTime = startTime.AddSeconds(i*_settings.TimeDelta);

                foreach (var node in nodes)
                {
                    var states = node.Send(currentTime);
                }

                if (ResourceProvider.MessagesUnDelivered == 0)
                {
                    stopSignal = true;
                }

            }

            ResourceProvider.MetricsLogger.WriteMessageMetrics(messages);
            Console.WriteLine(tag);
        }

        public void StopSimulation()
        {
            throw new NotImplementedException();
        }

        private NetworkSettings ReadNetworkSettings()
        {
            var json = File.ReadAllText(_configPath);
            var settings = JsonSerializer.Deserialize<Settings>(json);
            return settings!.NetworkSettings;
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
