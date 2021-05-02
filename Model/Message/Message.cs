using System;
using System.Collections.Generic;
using System.Text;

namespace NetSim.Model.Message
{
    public class Message
    {
        public string Data { get; set; }
        public float Size { get; set; }
        public string TargetId { get; set; }
    }
}
