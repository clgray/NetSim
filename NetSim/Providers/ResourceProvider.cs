using NetSim.Model;

namespace NetSim.Providers
{
    public static class ResourceProvider
    {
        public static ConnectionProvider ConnectionProvider { get; private set; }

        public static NodeProvider NodeProvider { get; private set; }

        public static RouterProvider RouterProvider { get; private set; }

        public static int MessagesUnDelivered { get; set; }
        public static int MessagesDeliverFailed { get; set; }


        public static void InitProviders(NetworkSettings settings)
        {
            ConnectionProvider = new ConnectionProvider(settings);
            NodeProvider = new NodeProvider(settings);
            RouterProvider = new RouterProvider(settings);
            MessagesDeliverFailed = 0;
            MessagesUnDelivered = settings.MessagesSettings.Quantity;
        }
    }
}
