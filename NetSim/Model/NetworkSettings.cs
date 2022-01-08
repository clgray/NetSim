using System;
using System.Collections.Generic;
using System.Text;
using NetSim.Model.Connection;
using NetSim.Model.Message;
using NetSim.Model.Node;

namespace NetSim.Model
{
	public class NetworkSettings
	{
		public float TimeDelta { get; set; }
		public List<NodeSettings> NodeSettings { get; set; }
		public List<ConnectionSettings> ConnectionSettings { get; set; }
		public MessagesSettings MessagesSettings { get; set; }
		public SimulationSettings SimulationSettings { get; set; }
	}

	public class SimulationSettings
	{
		public string RoutingAlgorithm { get; set; }
		public double MultiplierThresholdToBlock { get; set; }
		public double FractionThresholdToUnBlock { get; set; }
		public bool UseOnlyIsActiveNodes { get; set; }
		public int NumberOfGenerations { get; set; }
		public int MessagesToGenerateOnInit { get; set; }

		public override string ToString()
		{
			return
				$"RoutingAlgorithm {RoutingAlgorithm}, MultiplierThresholdToBlock {MultiplierThresholdToBlock}, FractionThresholdToUnBlock {FractionThresholdToUnBlock}, UseOnlyIsActiveNodes {UseOnlyIsActiveNodes}, NumberOfGenerations {NumberOfGenerations}";
		}
	}
}