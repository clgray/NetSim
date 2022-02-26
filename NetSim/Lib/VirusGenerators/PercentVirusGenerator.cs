using System;
using System.Linq;
using NetSim.Providers;

namespace NetSim.Lib.VirusGenerators
{
	public class PercentVirusGenerator : IVirusGenerator
	{
		private PercentVirusGeneratorSettings _generatorSettings;

		public void Init()
		{
			ReadGeneratorSettings();


			InfectNodesInternal(_generatorSettings.InfectNodesOnInit);
		}

		public void GenerateViruses(DateTime time)
		{
			InfectNodesInternal(_generatorSettings.InfeсtPercent);
			HealNodesInternal(_generatorSettings.HealPercent);
		}

		private void InfectNodesInternal(double infeсtPercent)
		{
			var rnd = new Random();
			var nodes = ResourceProvider.NodeProvider.GetNodes();

			var notInfectedNodes = nodes.Where(x => x.IsInfected() == false).ToArray();

			var quantity = Math.Min(notInfectedNodes.Length, nodes.Count * infeсtPercent);
			var infeсted = 0;
			while (infeсted < quantity)
			{
				var rndNode = notInfectedNodes[rnd.Next(notInfectedNodes.Length - 1)];
				if (!rndNode.IsInfected())
				{
					infeсted++;
					rndNode.Infect();
				}
			}
		}

		private void HealNodesInternal(double healPercent)
		{
			var rnd = new Random();
			var nodes = ResourceProvider.NodeProvider.GetNodes();

			var infectedNodes = nodes.Where(x => x.IsInfected()).ToArray();

			var quantity = Math.Min(infectedNodes.Length, nodes.Count * healPercent);
			var heal = 0;
			while (heal < quantity)
			{
				var rndNode = infectedNodes[rnd.Next(infectedNodes.Length - 1)];
				if (rndNode.IsInfected())
				{
					heal++;
					rndNode.Heal();
				}
			}
		}

		private void ReadGeneratorSettings()
		{
			_generatorSettings = new PercentVirusGeneratorSettings()
			{
				InfectNodesOnInit = ResourceProvider.SimulationSettings.VirusGeneratorSettings.InfectNodesOnInit,
				HealPercent = ResourceProvider.SimulationSettings.VirusGeneratorSettings.HealPercent,
				InfeсtPercent = ResourceProvider.SimulationSettings.VirusGeneratorSettings.InfeсtPercent
			};
		}
	}

	public class PercentVirusGeneratorSettings
	{
		public double InfectNodesOnInit { get; set; }
		public double HealPercent { get; set; }
		public double InfeсtPercent { get; set; }
	}
}