namespace NetSim.Model.Connection
{
    public class ConnectionMetrics
    {
        public int MessagesInQueue { get; set; }
        public int MessagesSent { get; set; }
        public float Load { get; set; }
        public float Throughput { get; set; }
    }
}
