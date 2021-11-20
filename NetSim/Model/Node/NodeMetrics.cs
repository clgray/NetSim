﻿using System;
using InfluxDB.Client.Core;

namespace NetSim.Model.Node
{
    [Measurement("node")]
    public class NodeMetrics
    {
        [Column("node", IsTag = true)]
        public string Id { get; set; }


        [Column("inqueue")]
        public int MessagesInQueue { get; set; }

        [Column("sent")]
        public int MessagesSent { get; set; }       
        
        [Column("received")]
        public int MessagesReceived { get; set; }

        [Column("sizesent")]
        public float MessagesSentSize { get; set; }

        [Column("sizereceived")]
        public float MessagesReceivedSize { get; set; }

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
