using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using NetSim.Model;
using System.Text.Json;
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
            InitNetwork();
        }

        public void StartSimulation()
        {
            var nodes = ResourceProvider.NodeProvider.GetNodes();
            var nodesIds = nodes.Select(x => x.GetId()).ToList();

            var messages = _messageGenerator.GenerateMessages(_settings.MessagesSettings, nodesIds);
            foreach (var message in messages)
            {
                var node = nodes.Find(x => x.GetId().Equals(message.StartId));
                node!.Receive(message);
            }

            var tag = Guid.NewGuid().ToString();
            var startTime = DateTime.UtcNow;
            var currentTime = startTime; // TODO: metrics logging
            var stopSignal = false;

            for (int i = 0; !stopSignal; i++)
            {
                currentTime = startTime.AddSeconds(i);

                foreach (var node in nodes)
                {
                    var states = node.Send();
                }

                if (ResourceProvider.MessagesUnDelivered == 0)
                {
                    stopSignal = true;
                }
            }
        }

        public void StopSimulation()
        {
            throw new NotImplementedException();
        }

        private NetworkSettings ReadNetworkSettings()
        {
            var json = File.ReadAllText(_configPath);
            var settings = JsonSerializer.Deserialize<NetworkSettings>(json);
            return settings;
        }

        private void InitNetwork()
        {
            ResourceProvider.InitProviders(_settings);
        }
    }
}
