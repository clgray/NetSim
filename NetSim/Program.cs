using System;
using System.IO;
using NetSim.Lib.MessageGenerators;
using NetSim.Lib.Networking;
using System.Text.Json;
using NetSim.Model;

namespace NetSim
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "networksettings.json");
            if (args.Length > 0)
            {
                path = args[0];
            }

            var json = File.ReadAllText(path);
            var networkSettings = JsonSerializer.Deserialize<NetworkSettings>(json);
            var network = new DefaultNetworking(networkSettings, new DefaultMessageGenerator());
            network.StartSimulation();
            Console.WriteLine("Симуляция завершена");
        }
    }
}
