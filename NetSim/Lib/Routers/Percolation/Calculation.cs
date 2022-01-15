using System;
using System.Collections.Generic;
using System.Numerics;
using static System.Math;

namespace NetSim.Lib.Routers.Percolation
{
	public static class Calculation
	{
		private static double ρ_1(double x, double t, double x0, double ε, double ξ, double τ, double L, int M)
		{
			var r = -2 / L * Exp(-Pow(ε - ξ, 2) * t / (2 * τ * (Pow(ε, 2) + Pow(ξ, 2)))) *
			        Exp((ε - ξ) * (x - x0) / (Pow(ε, 2) + Pow(ξ, 2)));
			double s = 0;
			for (var n = 1; n <= M; n++)
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
			for (var n = 1; n <= M; n++)
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
			for (var i = 1; i <= n; i++)
			{
				s += h * ρ_2(h * i, t, x0, ε, ξ, τ, L, M);
			}

			h = (L - x0) / n;
			for (var i = 1; i <= n; i++)
			{
				s += h * ρ_1(h * i + x0, t, x0, ε, ξ, τ, L, M);
			}

			return s;
		}

		public static double Intergal9(double t, double x0, double ε, double ξ, double τ, double L, int M, double l)
		{
			double n = 100;
			double h = x0 / n;
			Complex s = 0;
			for (var i = 1; i <= n; i++)
			{
				s += h * ρ_2_9(h * i, t, x0, ε, ξ, τ, L, M);
			}

			h = (l - x0) / n;
			for (var i = 1; i <= n; i++)
			{
				s += h * ρ_1_9(h * i + x0, t, x0, ε, ξ, τ, L, M);
			}

			return s.Real;
		}

		public static double SolveEquation6(double x0, double ε, double ξ, double τ, double L, int M, double λ)
		{
			var key = $"{x0}, {ε}, {ξ}, {τ}, {L}, {M}, {λ}";
			if (SolveEquation6Cache.ContainsKey(key))
				return SolveEquation6Cache[key];

			var value = Bisection(0.1, 1000, 0.01, t => 1.0 - λ - Intergal6(t, x0, ε, ξ, τ, L, M));
			SolveEquation6Cache[key] = value;
			return value;
		}

		public static double SolveEquation6_q(double x0, double ε, double ξ, double τ, double L, int M)
		{
			var key = $"{x0}, {ε}, {ξ}, {τ}, {L}, {M}";
			if (SolveEquation6Cache.ContainsKey(key))
				return SolveEquation6Cache[key];

			var value = Bisection(0.1, 1000, 0.01, t => Intergal6(t, x0, ε, ξ, τ, L, M) - 0.95);
			SolveEquation6Cache[key] = value;
			return value;
		}

		public static double SolveEquation9(double x0, double ε, double ξ, double τ, double L, int M, double l)
		{
			var key = $"{x0}, {ε}, {ξ}, {τ}, {L}, {M}, {l}";
			if (SolveEquation6Cache.ContainsKey(key))
				return SolveEquation9Cache[key];

			var value = Bisection(0.1, 1000, 0.01, t =>0.05 - Intergal9(t, x0, ε, ξ, τ, L, M, l));
			SolveEquation9Cache[key] = value;
			return value;
		}

		private static Complex ρ_1_9(double x, double t, double x0, double ε, double ξ, double τ, double L, int M)
		{
			var a = (Pow(ε, 2) + Pow(ξ, 2)) / 2 * τ;
			var b = (ε - ξ) / 4*τ;
			var k = b / a;

			var r = -2 / L * Exp(-t / τ) * Exp(k * (x - x0));

			Complex s = 0;
			
			for (var n = 1; n <= M; n++)
			{
				Complex d = ε * ξ / a * τ - 2 * a * τ * PI * PI * n * n / Pow(L, 2);
				Complex cosh = Complex.Cosh((t / τ) * Complex.Sqrt(d));
				s += Sin(PI * n * x0 / L) * Sin(PI * n * (L - x) / L) / Cos(PI * n) * cosh;
			}

			return r * s;
		}

		private static Complex ρ_2_9(double x, double t, double x0, double ε, double ξ, double τ, double L, int M)
		{
			var a = (Pow(ε, 2) + Pow(ξ, 2)) / 2 * τ;
			var b = (ε - ξ) / 4*τ;
			var k = b / a;

			var r = -2 / L * Exp(-t / τ) * Exp(k * (x - x0));

			Complex s = 0;
			for (var n = 1; n <= M; n++)
			{
				Complex d = ε * ξ / a * τ - 2 * a * τ * PI * PI * n * n / Pow(L, 2);
				Complex cosh = Complex.Cosh((t / τ) * Complex.Sqrt(d));
				s += Sin(PI * n * x / L) * Sin(PI * n * (L - x0) / L) / Cos(PI * n) * cosh;
			}

			return r * s;
		}

		private static readonly Dictionary<string, double> SolveEquation6Cache = new Dictionary<string, double>();
		private static readonly Dictionary<string, double> SolveEquation9Cache = new Dictionary<string, double>();

		static double Bisection(double x1, double x2, double precision, Func<double, double> f)

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
			} while (Abs(fx) > precision && Abs(x1 - x2) > precision);

			return x;
		}
	}
}