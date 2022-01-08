using System;
using System.Collections.Generic;
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
		private float _load;
		private float _messagesInQueueSize;
		private float _messagesReceiveSizePerStep;
		private int _messagesReceivePerStep;
		private NodeMetrics _nodeMetrics;

		public IpNode(NodeSettings settings, float timeDelta, string routingAlgorithm)
		{
			_settings = settings;
			_connections = new List<IConnection>();
			_router = ResourceProvider.RouterProvider.GetRouter(routingAlgorithm);
			_messageQueue = new Queue<Message>();
			_timeDelta = timeDelta;
			_nodeMetrics = new NodeMetrics();
		}

		public IEnumerable<State> ProgressQueue(DateTime currentTime)
		{
			float waitTimer = 0;

			var states = new List<State>();

			var nodeMetrics = new NodeMetrics()
			{
				MessagesInQueue = _messageQueue.Count,
				MessagesInQueueSize = _messagesInQueueSize,
				Throughput = _settings.Throughput,
				Time = currentTime,
				Id = GetId(),
				Tag = ResourceProvider.Tag,
				MessagesReceived = _messagesReceivePerStep,
				MessagesReceivedSize = _messagesReceiveSizePerStep,
				IsActive = IsActive()
			};
			_messagesReceiveSizePerStep = 0;
			_messagesReceivePerStep = 0;

			while (waitTimer < _timeDelta)
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

				_messagesInQueueSize -= message.Size;

				var nextNode = _router.GetRoute(this, message.TargetId, message);
				var timeSpent = CalculateTime(message);

				message.TimeSpent = (currentTime - message.Time).TotalSeconds;
				waitTimer += timeSpent;

				if (nextNode != null)
				{
					var connection = GetConnectionToNode(nextNode);

					var state = connection.Send(message, nextNode);
					message.State = MessageState.Sent; // Enqueue after fail?

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

			_load = waitTimer / _timeDelta;

			nodeMetrics.MessagesSent = nodeMetrics.MessagesInQueue - _messageQueue.Count;
			nodeMetrics.MessagesSentSize = nodeMetrics.MessagesInQueueSize - _messagesInQueueSize;
			nodeMetrics.Load = _load;
			if (nodeMetrics.Load > 1)
			{
				nodeMetrics.Load = 1;
			}

			_nodeMetrics = nodeMetrics;

			ResourceProvider.MetricsLogger.CollectNodeMetrics(nodeMetrics);

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
			_messagesReceiveSizePerStep += data.Size;
			_messagesReceivePerStep++;
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
			throw new NotSupportedException();
			// return (nodeLoad <= threshold) || _messagesInQueueSize <= _settings.Throughput * 10;
			// return _messagesInQueueSize <= _settings.Throughput * 50;
		}

		public float Load()
		{
			return _load;
		}

		public NodeMetrics GetNodeState()
		{
			return _nodeMetrics;
		}

		public List<IConnection> GetConnections()
		{
			return _connections;
		}

		public bool IsActive()
		{
			var L = _settings.Throughput * ResourceProvider.SimulationSettings.MultiplierThresholdToBlock;

			if (_messagesInQueueSize > L)
				_isActive = false;
			if (_messagesInQueueSize <= L * ResourceProvider.SimulationSettings.FractionThresholdToUnBlock)
				_isActive = true;
			return _isActive;
		}

		public void Disable()
		{
			_isActive = false;
		}

		private bool _isActive = true;


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