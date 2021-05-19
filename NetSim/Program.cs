using System;
using System.Collections.Generic;
using System.IO;
using NetSim.Lib.MessageGenerators;
using NetSim.Lib.Networking;
using System.Text.Json;
using NetSim.Model;
using NetSim.Model.Connection;
using NetSim.Model.Message;
using NetSim.Model.Node;

namespace NetSim
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "appsettings.json");
            if (args.Length > 0)
            {
                path = args[0];
            }
            var network = new DefaultNetworking(path, new DefaultMessageGenerator());
            network.StartSimulation();
            Console.WriteLine("Симуляция завершена");
        }
    }
}
