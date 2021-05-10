using System;
using System.Collections.Generic;
using System.Text;
using NetSim.Model.Connection;
using NetSim.Model.Message;
using NetSim.Providers;

namespace NetSim.Lib.Connections
{
    class WiredConnection : IConnection
    {
        private readonly ConnectionSettings _settings;
        private readonly List<INode> _connectedNodes;
        private bool IsActive { get; set; }
        

        public WiredConnection(ConnectionSettings settings, List<INode> connectedNodes)
        {
            _settings = settings;
            _connectedNodes = connectedNodes;
            IsActive = true;
        }

        public bool Send(Message data, INode receiver)
        {
            // TODO: message delay
            if (!IsActive)
            {
                ResourceProvider.MessagesUnDelivered -= 1;
                return false;
            }

            var timeSpent = CalculateTimeSpent(data); // TODO collect metrics
            receiver.Receive(data);
            return true;
        }

        public IEnumerable<INode> GetConnectedNodes()
        {
            return _connectedNodes;
        }

        public bool IsConnected(INode node)
        {
            return _connectedNodes.Contains(node);
        }

        public void Enable()
        {
            IsActive = true;
        }

        public void Disable()
        {
            IsActive = false;
        }

        public float GetBandwidth()
        {
            return _settings.Bandwidth;
        }

        private float CalculateTimeSpent(Message message)
        {
            return message.Size / _settings.Bandwidth;
        }
    }
}
