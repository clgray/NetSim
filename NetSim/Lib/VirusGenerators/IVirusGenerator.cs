using System;

namespace NetSim.Lib.VirusGenerators
{
	public interface IVirusGenerator
	{
		void Init();
		void GenerateViruses(DateTime time);
	}
}