using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace NetSim.Lib.Routers.Percalation
{
	class Calculation
	{
		public double ρ_1(double x, double t, double x0, double ε, double ξ, double τ, double L, int M)
		{
			var r = -2 / L * Exp(-Pow(ε - ξ, 2) * t / 2 * τ * (Pow(ε, 2) + Pow(ξ, 2))) *
			        Exp((ε - ξ) * (x - x0) / (Pow(ε, 2) + Pow(ξ, 2)));
			return 0;
		}
	}
	//ρ_1(x, t)=-2/L e-(ε-ξ)^2t2τ(ε2+ξ2)eε-ξx-x0ε2+ξ2n=1Msinπnx0LsinπnL-xLcos(πn)e- π2n2ε2+ξ2t2τL2
}
