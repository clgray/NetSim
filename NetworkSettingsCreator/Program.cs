using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CommandLine;
using NetworkSettingsCreator.Configuration;

namespace NetworkSettingsCreator
{
	class Program
	{
		static void Main(string[] args)
		{
			Parser.Default.ParseArguments<Options>(args)
				.WithNotParsed(HandleParseError)
				.MapResult(
					opts =>
					{
						try
						{
							opts.Validate();
							RunOptions(opts);
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

		static void RunOptions(Options opts)
		{
			var input = File.ReadAllText(opts.InputFile);
			var networkConfiguration = JsonSerializer.Deserialize<NetworkConfiguration>(input);
			var creator = new NetCreator.NetCreator();
			var net = creator.CreateNet(networkConfiguration);

			var output = JsonSerializer.Serialize(net);
			File.WriteAllText(opts.OutputFile, output);
		}

		static void HandleParseError(IEnumerable<Error> errs)
		{
			//handle errors
		}
	}
}