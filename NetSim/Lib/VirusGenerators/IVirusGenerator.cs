using System;
using NetSim.Model;

namespace NetSim.Lib.VirusGenerators
{
	public interface IVirusGenerator
	{
		void Init();
		void GenerateViruses(DateTime time);
	}
}