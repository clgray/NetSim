using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSim.Lib.Routers.Percalation;

namespace NetSimTests.Percalation
{
	[TestClass]
	public class CalculationTests
	{
		[TestMethod]
		public void Intergal6Test()
		{
			var p = Calculation.Intergal6(4, 50, 15, 7, 1, 90, 50);
			for (int i = 0; i < 100; i++)
			{
				Console.WriteLine(Calculation.Intergal6(i/10.0, 50, 15, 7, 1, 100, 100));
			}

			Assert.AreEqual(42, p);
		}

		[TestMethod]
		public void SolveEquation6Test()
		{
			var x = Calculation.SolveEquation6(50, 15, 7, 1, 90, 50);

			Assert.AreEqual(42, x);
		}
	}
}
