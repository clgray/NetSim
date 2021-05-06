using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NetSim.Model;
using NetSim.Model.Message;
using NetSim.Model.Node;

namespace NetSim.Lib.Nodes
{
    public class IpNode : INode
    {
        private readonly Queue<Message> _messageQueue;
        private readonly NodeSettings _settings;
        private readonly IRouter _router;
        private readonly List<IConnection> _connections;

        public IpNode(NodeSettings settings)
        {
            _settings = settings;
            _connections = new List<IConnection>();
            // TODO: create _router from settings algorithm a.k.a. fabric
            _messageQueue = new Queue<Message>();
        }

        public IpNode(NodeSettings settings, IEnumerable<Message> messages)
        {
            _settings = settings;
            _connections = new List<IConnection>();

            _messageQueue = new Queue<Message>(messages);
        }

        public State Send()
        {
            var haveMessageToSend = _messageQueue.TryDequeue(out var message);

            if (!haveMessageToSend)
            {
                return new State()
                {
                    Message = $"No messages to send",
                    Node = _settings.Id,
                    Success = true
                };
            }

            var nextNode = _router.GetRoute(this, message.TargetId);
            var connection = GetConnectionToNode(nextNode);
            var timeSpent = CalculateTime(message);

            if (nextNode != null)
            {
                var state = connection.Send(message, nextNode);
                if (!state)
                {
                    return new State()
                    {
                        Message = $"Message not sent from node {_settings.Id}. Reason: Connection offline",
                        Node = _settings.Id,
                        Success = false,
                        TimeSpent = timeSpent
                    };
                }
                return new State()
                {
                    Message = $"Message sent from node {_settings.Id}",
                    Node = _settings.Id,
                    Success = true,
                    TimeSpent = timeSpent
                };
            }

            return new State()
            {
                Message = $"Message not sent from node {_settings.Id}. Reason: No route",
                Node = _settings.Id,
                Success = false,
                TimeSpent = timeSpent
            };
        }

        public State Receive(Message data)
        {
            _messageQueue.Enqueue(data);
            return new State()
            {
                Message = $"Message received on node {_settings.Id}",
                Node = _settings.Id,
                Success = true,
            };
        }

        public string GetId()
        {
            return _settings.Id;
        }

        public void AddConnection(IConnection connection)
        {
            _connections.Add(connection);
        }

        private float CalculateTime(Message message)
        {
            return message.Size/_settings.Throughput;
        }

        private IConnection GetConnectionToNode(INode node)
        {
            return _connections.Find(x => x.IsConnected(node));
        }
    }
}
