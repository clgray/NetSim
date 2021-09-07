﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NetSim.Model.Message
{
    public class Message
    {
        public string Data { get; set; }
        public float Size { get; set; }
        public float TimeSpent { get; set; } = 0;
        public DateTime StartTime { get; set; }
        public DateTime DeliveredTime { get; set; }
        public string StartId { get; set; }
        public string TargetId { get; set; }
        public List<string> Path { get; set; } = new List<string>();

        public MessageState State { get; set; }

    }

    public enum MessageState
    {
        New,
        Received,
        Sent,
        Failed,
        Delivered
    }
}
