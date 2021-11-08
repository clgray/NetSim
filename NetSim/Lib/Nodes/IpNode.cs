using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using InfluxDB.Client.Api.Domain;
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
        private float _load;
        private float _messagesInQueueSize;
        private int _messagesInQueueCount;
        private NodeMetrics _nodeMetrics;

        public IpNode(NodeSettings settings, float timeDelta)
        {
            _settings = settings;
            _connections = new List<IConnection>();
            _router = ResourceProvider.RouterProvider.GetRouter(_settings.RoutingAlgorithm);
            _messageQueue = new Queue<Message>();
            _timeDelta = timeDelta;
            _waitTimer = 0;
            _messagesInQueueCount = 0;
            _messagesInQueueSize = 0;
            _nodeMetrics = new NodeMetrics();
        }

        public IpNode(NodeSettings settings, IEnumerable<Message> messages)
        {
            _settings = settings;
            _connections = new List<IConnection>();

            _messageQueue = new Queue<Message>(messages);
        }

        public IEnumerable<State> Send(DateTime currentTime)
        {
            if (_waitTimer < 0)
            {
                _waitTimer = 0;
            }

            var states = new List<State>();

            var nodeMetrics = new NodeMetrics()
            {
                MessagesInQueue = _messageQueue.Count,
                Throughput = _settings.Throughput,
                Time = currentTime,
                Id = GetId(),
                Tag = ResourceProvider.Tag,
                MessagesReceived = _messageQueue.Count - _messagesInQueueCount
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

                var nextNode = _router.GetRoute(this, message.TargetId, message);
                var timeSpent = CalculateTime(message);
                message.TimeSpent += timeSpent;
                _waitTimer += timeSpent;

                if (nextNode != null)
                {
                    var connection = GetConnectionToNode(nextNode);

                    var state = connection.Send(message, nextNode);
                    message.State = MessageState.Sent; // Enqueue after fail?

                    _messagesInQueueSize -= message.Size;

                    if (!state)
                    {
                        message.State = MessageState.Failed; // TODO: resent message in case of dead Connection
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
                ResourceProvider.MessagesUnDelivered -= 1;
                states.Add(new State()
                {
                    Message = $"Message not sent from node. Reason: No route",
                    Node = _settings.Id,
                    Success = false,
                    TimeSpent = timeSpent
                });
            }

            _load = _waitTimer / _timeDelta;
            _messagesInQueueCount = _messageQueue.Count;

            nodeMetrics.MessagesSent = nodeMetrics.MessagesInQueue - _messagesInQueueCount;
            nodeMetrics.MessagesInQueue = _messagesInQueueCount;
            nodeMetrics.MessagesTotalSize = _messagesInQueueSize;
            nodeMetrics.Load = _load;
            if (nodeMetrics.Load > 1)
            {
                nodeMetrics.Load = 1;
            }

            _nodeMetrics = nodeMetrics;

            ResourceProvider.MetricsLogger.CollectNodeMetrics(nodeMetrics);

            _waitTimer -= _timeDelta;

            foreach (var connection in _connections)
            {
                connection.ProgressQueue(currentTime);
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
            _messagesInQueueSize += data.Size;
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

        public bool IsAvailable()
        {
            // move to settings maybe
            var threshold = 0.9;
            var nodeLoad = _load;

            return !(nodeLoad > threshold);
        }

        public NodeMetrics GetNodeState()
        {
            return _nodeMetrics;
        }

        public List<IConnection> GetConnections()
        {
            return _connections;
        }

        private float CalculateTime(Message message)
        {
            return message.Size / _settings.Throughput;
        }

        private IConnection GetConnectionToNode(INode node)
        {
            return _connections.Find(x => x.IsConnected(node));
        }
    }
}
