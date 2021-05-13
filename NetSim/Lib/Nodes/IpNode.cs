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
using NetSim.Providers;

namespace NetSim.Lib.Nodes
{
    public class IpNode : INode
    {
        private readonly Queue<Message> _messageQueue;
        private readonly NodeSettings _settings;
        private readonly IRouter _router;
        private readonly List<IConnection> _connections;
        private readonly float _timeDelta;
        private float _waitTimer;

        public IpNode(NodeSettings settings, float timeDelta)
        {
            _settings = settings;
            _connections = new List<IConnection>();
            _router = ResourceProvider.RouterProvider.GetRouter(_settings.RoutingAlgorithm);
            _messageQueue = new Queue<Message>();
            _timeDelta = timeDelta;
            _waitTimer = 0;
        }

        public IpNode(NodeSettings settings, IEnumerable<Message> messages)
        {
            _settings = settings;
            _connections = new List<IConnection>();

            _messageQueue = new Queue<Message>(messages);
        }

        public IEnumerable<State> Send()
        {
            if (_waitTimer < 0)
            {
                _waitTimer = 0;
            }

            var states = new List<State>();

            var nodeMetrics = new NodeMetrics()
            {
                MessagesInQueue = _messageQueue.Count,
                Throughput = _settings.Throughput
            };

            while (_waitTimer < _timeDelta)
            {
                var haveMessageToSend = _messageQueue.TryDequeue(out var message);
                
                if (!haveMessageToSend)
                {
                    states.Add(new State()
                    {
                        Message = $"No messages to send",
                        Node = _settings.Id,
                        Success = true
                    });
                    break;
                }

                var nextNode = _router.GetRoute(this, message.TargetId);
                var timeSpent = CalculateTime(message);
                _waitTimer += timeSpent;

                if (nextNode != null)
                {
                    var connection = GetConnectionToNode(nextNode);

                    var state = connection.Send(message, nextNode);
                    message.State = MessageState.Sent; // Enqueue after fail?
                    if (!state)
                    {
                        message.State = MessageState.Failed; // TODO: resent message in case of dead connection
                        ResourceProvider.MessagesDeliverFailed += 1;
                        states.Add(new State()
                        {
                            Message = $"Message not sent from node. Reason: Connection offline",
                            Node = _settings.Id,
                            Success = false,
                            TimeSpent = timeSpent
                        });
                        continue;
                    }
                    states.Add(new State()
                    {
                        Message = $"Message sent from node",
                        Node = _settings.Id,
                        Success = true,
                        TimeSpent = timeSpent
                    });
                    continue;
                }

                message.State = MessageState.Failed;
                ResourceProvider.MessagesDeliverFailed += 1;
                states.Add(new State()
                {
                    Message = $"Message not sent from node. Reason: No route",
                    Node = _settings.Id,
                    Success = false,
                    TimeSpent = timeSpent
                });
            }

            nodeMetrics.MessagesSent = nodeMetrics.MessagesInQueue - _messageQueue.Count;
            nodeMetrics.Load = _waitTimer / _timeDelta;
            // TODO: log metrics

            _waitTimer -= _timeDelta;

            foreach (var connection in _connections)
            {
                connection.ProgressQueue();
            }

            return states;
        }

        public State Receive(Message data)
        {
            data.State = MessageState.Received;
            data.Path.Add(_settings.Id);
            if (data.TargetId.Equals(_settings.Id))
            {
                ResourceProvider.MessagesUnDelivered -= 1;
                return new State()
                {
                    Message = $"Message delivered",
                    Node = _settings.Id,
                    Success = true,
                    TimeSpent = 0
                };
            }

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
        
        public List<IConnection> GetConnections()
        {
            return _connections;
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
