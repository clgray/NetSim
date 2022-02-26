using System;
using System.Collections.Generic;
using NetSim.Model.Message;
using NetSim.Providers;

namespace NetSim.Lib.MessageGenerators
{
    public class ConstantMessageGenerator : IMessageGenerator
    {
        private MessagesSettings _settings;
        private ConstantMessageGeneratorSettings _generatorSettings;
        private List<string> _nodeIds;
        private int _numberOfGenerations;

        public List<Message> Init(MessagesSettings settings, List<string> nodeIds, DateTime time)
        {
            _settings = settings;
            _nodeIds = nodeIds;
            _numberOfGenerations = 0;

            ReadGeneratorSettings();
            
            if (_generatorSettings.MessagesToGenerateOnInit == 0)
            {
                return new List<Message>();
            }


            return GenerateMessagesInternal(time, true);
        }

        public List<Message> GenerateMessages(DateTime time)
        {
            if (_numberOfGenerations < _generatorSettings.NumberOfGenerations)
            {
                _numberOfGenerations += 1;
                return GenerateMessagesInternal(time, false);
            }
            return new List<Message>();
        }

        public bool GenerateInProgress()
        {
            return true;
        }

        private List<Message> GenerateMessagesInternal(DateTime time, bool isInit)
        {
            var rnd = new Random(_settings.Seed);
            var messages = new List<Message>();
            var quantity = _settings.Quantity;

            if (isInit)
            {
                quantity = _generatorSettings.MessagesToGenerateOnInit;
            }

            for (int i = 0; i < quantity; i++)
            {
                var size = Math.Abs(rnd.Next(_settings.MinSize, _settings.MaxSize));

                var rndNodeId = rnd.Next(_nodeIds.Count);
                var nodeId = _nodeIds[rndNodeId];

                var targetNodeId = rnd.Next(_nodeIds.Count);
                while (targetNodeId == rndNodeId)
                {
                    targetNodeId = rnd.Next(_nodeIds.Count);
                }
                var targetId = _nodeIds[targetNodeId];

                messages.Add(new Message { Data = i.ToString(), Size = size, State = MessageState.New, TargetId = targetId, StartId = nodeId, Time = time });
            }

            return messages;
        }

        private void ReadGeneratorSettings()
        {
           _generatorSettings = new ConstantMessageGeneratorSettings()
           {
               MessagesToGenerateOnInit = ResourceProvider.SimulationSettings.MessagesToGenerateOnInit,
               NumberOfGenerations = ResourceProvider.SimulationSettings.NumberOfGenerations
           };
        }
    }
}
