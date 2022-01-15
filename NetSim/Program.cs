using System;
using System.Collections.Generic;
using System.IO;
using NetSim.Lib.MessageGenerators;
using NetSim.Lib.Networking;
using System.Text.Json;
using CommandLine;
using NetSim.Model;
using Serilog;

namespace NetSim
{
	class Program
	{
		static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.WriteTo.Console()
				.WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day)
				.CreateLogger();

			Parser.Default.ParseArguments<Options>(args)
				.WithNotParsed(HandleParseError)
				.MapResult(
					opts =>
					{
						try
						{
							Run(opts);
							return 0;
						}
						catch (ArgumentValidationException e)
						{
							Console.WriteLine("Invalid arguments detected: {0}", e.Message);
							return 1;
						}
					},
					errs => 1);
		}

		private static void HandleParseError(IEnumerable<Error> obj)
		{
			throw new NotImplementedException();
		}

		static void Run(Options options)
		{
			var json = File.ReadAllText(options.InputFile);
			var networkSettings = JsonSerializer.Deserialize<NetworkSettings>(json);
			networkSettings.SimulationSettings ??= new SimulationSettings();

			InitSettings(options, networkSettings);

			var network = new DefaultNetworking(networkSettings, new ConstantMessageGenerator());
			Log.Information("Start");
			Log.Information(networkSettings.SimulationSettings.ToString());
			Log.Information("Quantity " + networkSettings.MessagesSettings.Quantity);
			network.StartSimulation();
			Log.Information("Симуляция завершена");
		}

		private static void InitSettings(Options options, NetworkSettings networkSettings)
		{
			networkSettings.SimulationSettings.RoutingAlgorithm =
				options.RoutingAlgorithm ?? networkSettings.SimulationSettings.RoutingAlgorithm;

			networkSettings.SimulationSettings.FractionThresholdToUnBlock =
				options.FractionThresholdToUnBlock ?? networkSettings.SimulationSettings.FractionThresholdToUnBlock;

			networkSettings.SimulationSettings.MultiplierThresholdToBlock =
				options.MultiplierThresholdToBlock ?? networkSettings.SimulationSettings.MultiplierThresholdToBlock;

			networkSettings.SimulationSettings.NumberOfGenerations =
				options.NumberOfGenerations ?? networkSettings.SimulationSettings.NumberOfGenerations;
			networkSettings.SimulationSettings.MessagesToGenerateOnInit =
				options.MessagesToGenerateOnInit ?? networkSettings.SimulationSettings.MessagesToGenerateOnInit;

			networkSettings.SimulationSettings.UseOnlyIsActiveNodes =
				options.UseOnlyIsActiveNodes ?? networkSettings.SimulationSettings.UseOnlyIsActiveNodes;
			networkSettings.MessagesSettings.Quantity =
				options.MessagesQuantity ?? networkSettings.MessagesSettings.Quantity;

			networkSettings.SimulationSettings.λ = options.λ;
		}
	}
}