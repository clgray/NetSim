using System;
using static System.Math;

namespace NetSim.Lib.Routers.Percalation
{
	public class Calculation
	{
		private static double ρ_1(double x, double t, double x0, double ε, double ξ, double τ, double L, int M)
		{
			var r = -2 / L * Exp(-Pow(ε - ξ, 2) * t / (2 * τ * (Pow(ε, 2) + Pow(ξ, 2)))) *
			        Exp((ε - ξ) * (x - x0) / (Pow(ε, 2) + Pow(ξ, 2)));
			double s = 0;
			for (int n = 1; n <= M; n++)
			{
				s += Sin(PI * n * x0 / L) * Sin(PI * n * (L - x) / L) / Cos(PI * n) *
				     Exp(-PI * PI * n * n * (ε * ε + ξ * ξ) * t / (2 * τ * L * L));
			}

			return r * s;
		}

		private static double ρ_2(double x, double t, double x0, double ε, double ξ, double τ, double L, int M)
		{
			var r = -2 / L * Exp(-Pow(ε - ξ, 2) * t / (2 * τ * (Pow(ε, 2) + Pow(ξ, 2)))) *
			        Exp((ε - ξ) * (x - x0) / (Pow(ε, 2) + Pow(ξ, 2)));
			double s = 0;
			for (int n = 1; n <= M; n++)
			{
				s += Sin(PI * n * x / L) * Sin(PI * n * (L - x0) / L) / Cos(PI * n) *
				     Exp(-PI * PI * n * n * (ε * ε + ξ * ξ) * t / (2 * τ * L * L));
			}

			return r * s;
		}


		public static double Intergal6(double t, double x0, double ε, double ξ, double τ, double L, int M)
		{
			double n = 100;
			double h = x0 / n;
			double s = 0;
			for (int i = 1; i <= n; i++)
			{
				s += h * ρ_2(h * i, t, x0, ε, ξ, τ, L, M);
			}

			h = (L - x0) / n;
			for (int i = 1; i <= n; i++)
			{
				s += h * ρ_1(h * i + x0, t, x0, ε, ξ, τ, L, M);
			}

			return s;
		}

		public static double SolveEquation6(double x0, double ε, double ξ, double τ, double L, int M)
		{
			return Secant(1, 1000, 0.05, t => Intergal6(t, x0, ε, ξ, τ, L, M) - 0.1);
		}

		static double Secant(double x1, double x2, double precision, Func<double, double> f)

		{
			var fx1 = f(x1);
			var fx2 = f(x2);

			if (fx1 * fx2 > 0.0D)
			{
				return -1;
			}

			double fx;
			double x;
			do
			{
				x = (x1 + x2) / 2.0;
				fx2 = f(x2);
				fx = f(x);

				if (fx * fx2 > 0.0D)
				{
					x2 = x;
				}
				else
				{
					x1 = x;
				}

			} while (Abs(fx) > precision);

			return x;
		}
	}
}