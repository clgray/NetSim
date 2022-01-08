using System;
using System.IO;
using CommandLine;
using CsvHelper.Configuration.Attributes;

namespace NetSim
{
	class Options
	{
		[Option('i', "input", Required = true, Default = "networkSettings.json",
			HelpText = "Input file with NetworkSettings.")]
		public string InputFile { get; set; }

		[Option('r', "routing", Required = false, HelpText = "Routing Algorithm", Default = "dijkstraPath")]
		public string RoutingAlgorithm { get; set; }

		[Option('l', "MultiplierThresholdToBlock", Required = false, Default = 50.0,
			HelpText = "MultiplierThresholdToBlock")]
		public double? MultiplierThresholdToBlock { get; set; }

		[Option('f', "FractionThresholdToUnBlock", Required = false, Default = 0.5,
			HelpText = "FractionThresholdToUnBlock")]

		public double? FractionThresholdToUnBlock { get; set; }

		[Option('a', "UseOnlyIsActiveNodes", Required = false, Default = true, HelpText = "UseOnlyIsActiveNodes")]

		public bool? UseOnlyIsActiveNodes { get; set; }

		[Option('n', "NumberOfGenerations", Required = false, Default = 120,
			HelpText = "ConstantMessageGeneratorSettings NumberOfGenerations")]
		public int? NumberOfGenerations { get; set; }


		[Option('n', "MessagesToGenerateOnInit", Required = false, Default = 1,
			HelpText = "ConstantMessageGeneratorSettings MessagesToGenerateOnInit")]
		public int? MessagesToGenerateOnInit { get; set; }

		[Option('q', "MessagesQuantity", Required = false,
			HelpText = "MessagesSettings Quantity")]
		public int? MessagesQuantity { get; set; }
		
	}

	class ArgumentValidationException : Exception
	{
		public ArgumentValidationException(string message) : base(message)
		{
		}
	}
}