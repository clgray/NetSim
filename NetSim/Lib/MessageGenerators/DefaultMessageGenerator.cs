using System;
using System.Collections.Generic;
using System.Text;
using NetSim.Model;
using NetSim.Model.Message;

namespace NetSim.Lib.MessageGenerators
{
    public class DefaultMessageGenerator : IMessageGenerator
    {
        private List<Message> GenerateMessages(MessagesSettings settings, List<string> nodeIds, DateTime time)
        {
            var rnd = new Random(settings.Seed);
            var messages = new List<Message>();

            for (int i = 0; i < settings.Quantity; i++)
            {
                var size = Math.Abs(rnd.Next(settings.MinSize, settings.MinSize + settings.MaxSize));

                var rndNodeId = rnd.Next(nodeIds.Count);
                var nodeId = nodeIds[rndNodeId];

                var targetNodeId = rnd.Next(nodeIds.Count);
                while (targetNodeId == rndNodeId)
                {
                    targetNodeId = rnd.Next(nodeIds.Count);
                }
                var targetId = nodeIds[targetNodeId];

                messages.Add(new Message {Data = i.ToString(), Size = size, State = MessageState.New, TargetId = targetId, StartId = nodeId, Time = time});
            }

            return messages;
        }

        public List<Message> Init(MessagesSettings settings, List<string> nodeIds, DateTime time)
        {
            return GenerateMessages(settings, nodeIds, time);
        }

        public List<Message> GenerateMessages(DateTime time)
        {
            throw new NotImplementedException();
        }

        public bool GenerateInProgress()
        {
            return false;
        }
    }
}
