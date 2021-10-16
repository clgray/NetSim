using System;
using System.Collections.Generic;
using System.Text;
using NetSim.Model;
using NetSim.Model.Message;

namespace NetSim.Lib
{
    public interface IMessageGenerator
    {
        List<Message> Init(MessagesSettings settings, List<string> nodeIds, DateTime time);
        List<Message> GenerateMessages(DateTime time);
        bool GenerateInProgress();
    }
}
