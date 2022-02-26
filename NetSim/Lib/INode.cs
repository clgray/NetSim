using NetSim.Model.Message;
using System;
using System.Collections.Generic;
using NetSim.Model;
using NetSim.Model.Node;

namespace NetSim.Lib
{
	public interface INode
	{
		public IEnumerable<State> ProgressQueue(DateTime currentTime);
		public State Receive(Message data);
		public string GetId();
		public List<IConnection> GetConnections();
		public void AddConnection(IConnection connection);
		public bool IsInfected();
		public float Load();
		public NodeMetrics GetNodeState();
		public bool IsActive();
		void Disable();
		void IterationStart();
		void IterationEnd();
		public bool IsBlockedOnStep { get;}
		public bool IsUnBlockedOnStep { get;}
		void Infect();
		void Heal();
	}
}
