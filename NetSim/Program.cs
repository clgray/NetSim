using System;
using System.IO;
using NetSim.Lib.Networking;

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
            var network = new DefaultNetworking(path);
            network.StartSimulation();
        }
    }
}
