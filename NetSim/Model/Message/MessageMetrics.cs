using System;
using InfluxDB.Client.Core;

namespace NetSim.Model.Message
{
    [Measurement("message")]
    public class MessageMetrics
    {
        [Column("data", IsTag = false)]
        public string data { get; set; }
        [Column("size")]
        public float Size { get; set; }
        [Column("timespent")]
        public float TimeSpent { get; set; }
        [Column("path")]
        public string Path { get; set; }
        [Column("received")]
        public bool Received { get; set; }
        [Column("time", IsTimestamp = true)]
        public DateTime Time { get; set; }
        [Column("tag", IsTag = true)]
        public string Tag { get; set; }

        public override string ToString()
        {
            var str = $"{data},{Size},{TimeSpent},{Path},{Received},{Time}";
            return str;
        }
    }
}
