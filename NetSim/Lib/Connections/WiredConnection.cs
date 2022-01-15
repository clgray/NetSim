using System;
using System.Collections.Generic;
using NetSim.Model.Connection;
using NetSim.Model.Message;
using NetSim.Providers;

namespace NetSim.Lib.Connections
{
	class WiredConnection : IConnection
	{
		private readonly ConnectionSettings _settings;
		private readonly List<INode> _connectedNodes;
		private readonly Queue<MessageData> _queue;
		private readonly float _timeDelta;
		private float _load;
		private float _messageInQueueSize;
		private int _messageInQueueCount;
		private bool IsActive { get; set; }

		private class MessageData
		{
			public INode Receiver { get; set; }
			public float Delay { get; set; }
			public Message Message { get; set; }
		}

		public double TimeWaiting => _messageInQueueSize / _settings.Bandwidth;

		public WiredConnection(ConnectionSettings settings, List<INode> connectedNodes, float timeDelta)
		{
			_settings = settings;
			_connectedNodes = connectedNodes;
			_queue = new Queue<MessageData>();
			_timeDelta = timeDelta;
			IsActive = true;
			_messageInQueueCount = 0;
			_messageInQueueSize = 0;
		}

		public bool Send(Message data, INode receiver)
		{
			if (!IsActive)
			{
				ResourceProvider.MessagesUnDelivered -= 1;
				return false;
			}

			var timeSpent = CalculateTimeSpent(data);

			_queue.Enqueue(new MessageData
			{
				Delay = timeSpent,
				Message = data,
				Receiver = receiver
			});
			_messageInQueueSize += data.Size;

			return true;
		}

		public float GetLoad()
		{
			return _load;
		}

		public void ProgressQueue(DateTime currentTime)
		{
			if (!IsActive)
			{
				return;
			}

			float waitTimer = 0;


			var connectionMetrics = new ConnectionMetrics
			{
				MessagesInQueue = _queue.Count,
				Throughput = _settings.Bandwidth,
				Time = currentTime,
				Connection = string.Join('-', _settings.NodeIds),
				Tag = ResourceProvider.Tag,
				MessagesReceived = _queue.Count - _messageInQueueCount
			};

			while (waitTimer < _timeDelta)
			{
				var dataToTransmit = _queue.TryPeek(out var data);

				if (!dataToTransmit)
				{
					break;
				}

				var capacity = _timeDelta - waitTimer < 0 ? 0 : _timeDelta - waitTimer;
				var workTime = Math.Min(data.Delay, capacity);
				waitTimer += workTime;

				data.Delay -= workTime;
				if (data.Delay <= 0)
				{
					data.Message.TimeSpent = (currentTime - data.Message.Time).TotalSeconds;
					data.Receiver.Receive(data.Message);
					_queue.Dequeue();
				}

			}

			_load = waitTimer / _timeDelta;
			_messageInQueueCount = _queue.Count;
			// Possible error accumulation here
			_messageInQueueSize -= waitTimer * _settings.Bandwidth;

			connectionMetrics.MessagesSent = connectionMetrics.MessagesInQueue - _messageInQueueCount;
			connectionMetrics.MessagesInQueue = _messageInQueueCount;
			connectionMetrics.MessagesTotalSize = _messageInQueueSize;
			connectionMetrics.Load = _load;
			if (connectionMetrics.Load > 1)
			{
				connectionMetrics.Load = 1;
			}

			ResourceProvider.MetricsLogger.CollectConnectionMetrics(connectionMetrics);
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