using System;
using InfluxDB.Client.Core;

namespace NetSim.Model.Connection
{
    [Measurement("connection")]
    public class ConnectionMetrics
    {
        [Column("connectionnodes", IsTag = true)]
        public string Connection { get; set; }
        [Column("inqueue")]
        public int MessagesInQueue { get; set; }
        [Column("sent")]
        public int MessagesSent { get; set; }        
        [Column("received")]
        public int MessagesReceived { get; set; }
        [Column("totalsize")]
        public float MessagesTotalSize { get; set; }
        [Column("load")]
        public float Load { get; set; }
        [Column("throughput")]
        public float Throughput { get; set; }
        [Column("time", IsTimestamp = true)]
        public DateTime Time { get; set; }
        [Column("tag", IsTag = true)]
        public string Tag { get; set; }
    }
}
