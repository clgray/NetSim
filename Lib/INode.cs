using NetSim.Model.Message;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetSim.Lib
{
    public interface INode
    {
        public Task<bool> Send(Message data);
        public Task<bool> Receive(Message data);
    }
}
