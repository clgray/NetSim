using System;
using System.Collections.Generic;
using System.Text;
using NetSim.Model.Message;

namespace NetSim.Lib.MessageGenerators
{
    public class DefaultMessageGenerator : IMessageGenerator
    {
        public List<Message> GenerateMessages(MessagesSettings settings)
        {
            var rnd = new Random(settings.Seed);
            var messages = new List<Message>();

            for (int i = 0; i < settings.Quantity; i++)
            {
                var size = Math.Abs(rnd.Next(settings.Size, settings.Size + settings.SizeRange));

                messages.Add(new Message {Data = i.ToString(), Size = size});
            }

            return messages;
        }
    }
}
