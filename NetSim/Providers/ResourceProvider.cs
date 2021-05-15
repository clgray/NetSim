using System.ComponentModel;
using InfluxDB.Collector;
using NetSim.Lib;
using NetSim.Model;
using NetSim.Repository;

namespace NetSim.Providers
{
    public static class ResourceProvider
    {
        public static ConnectionProvider ConnectionProvider { get; private set; }
        public static NodeProvider NodeProvider { get; private set; }
        public static RouterProvider RouterProvider { get; private set; }
        
        public static MetricsLogger MetricsLogger { get; private set; }

        public static int MessagesUnDelivered { get; set; }
        public static int MessagesDeliverFailed { get; set; }

        public static string Tag { get; set; }


        public static void InitProviders(NetworkSettings settings, string tag)
        {
            RouterProvider = new RouterProvider(settings);
            ConnectionProvider = new ConnectionProvider(settings);
            NodeProvider = new NodeProvider(settings);
            ConnectionProvider.GenerateConnections();

            Tag = tag;
            var dbProvider = new InfluxDBProvider(tag);
            MetricsLogger = new MetricsLogger(dbProvider);

            MessagesDeliverFailed = 0;
            MessagesUnDelivered = settings.MessagesSettings.Quantity;
        }
    }
}
