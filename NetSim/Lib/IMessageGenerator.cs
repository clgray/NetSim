using System;
using System.Collections.Generic;
using System.Text;
using NetSim.Model.Message;

namespace NetSim.Lib
{
    public interface IMessageGenerator
    {
        List<Message> GenerateMessages(MessagesSettings settings, List<string> nodeIds);
    }
}
