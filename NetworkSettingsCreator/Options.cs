using System;
using System.IO;
using CommandLine;

namespace NetworkSettingsCreator
{
	class Options
	{
		[Option('i', "input", Required = false, Default = "networkConfiguration.json",
			HelpText = "Input file with NetworkConfiguration.")]
		public string InputFile { get; set; }

		[Option('o', "output", Required = false, Default = "networkSettings.json",
			HelpText = "Output file with NetworkSettings.")]
		public string OutputFile { get; set; }

		public void Validate()
		{
			if (!File.Exists(InputFile))
			{
				throw new ArgumentValidationException($"{InputFile} path does not exist!");
			}
		}
	}
	class ArgumentValidationException : Exception
	{
		public ArgumentValidationException(string message) : base(message)
		{
		}
	}
}