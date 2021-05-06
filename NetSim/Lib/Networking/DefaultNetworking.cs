using System;
using System.Collections.Generic;
using System.IO;
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

        public DefaultNetworking(string configPath)
        {
            _configPath = configPath;
            _settings = ReadNetworkSettings();
            InitNetwork();
        }

        public void StartSimulation()
        {
            ResourceProvider.InitProviders(_settings);

            var nodes = ResourceProvider.NodeProvider.GetNodes();

            var tag = Guid.NewGuid().ToString();
            var startTime = DateTime.UtcNow;
            var currentTime = startTime;
            var stopSignal = false;
            
            for (int i = 0; !stopSignal; i++)
            {
                currentTime = startTime.AddSeconds(i);

                foreach (var node in nodes)
                {
                    var state = node.Send();
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
            // TODO
        }
    }
}
