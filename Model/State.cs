using System;
using System.Collections.Generic;
using System.Text;

namespace NetSim.Model
{
    public class State
    {
        public string Message { get; set; } = string.Empty;
        public string Node { get; set; } = string.Empty;
        public bool Success { get; set; } = default;
        public float TimeSpent { get; set; } = 0;
    }
}
