using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSim.Model.Message;

namespace NetSim.Lib.Routers
{
    internal class DijkstraRouterHelper
    {
        public Dictionary<Message, List<DijkstraNode>> Paths { get; set; }

        public DijkstraRouterHelper()
        {
            Paths = new Dictionary<Message, List<DijkstraNode>>();
        }

        public void SavePath(Message message, List<DijkstraNode> path)
        {
            Paths.Add(message, path);
        }

        public INode GetNextNodeFromSavedPath(Message message, DijkstraNode currentNode)
        {
            if (!Paths.ContainsKey(message))
            {
                return null;
            }
            var path = Paths[message];
            var currentIndex = path.FindIndex(x => x.Equals(currentNode));

            
            // This means what next node is destination, which means message is delivered
            // In case when next node is not accessible from current node means what this path is also irrelevant and should non longer be in memory
            if (currentIndex + 2 == path.Count)
            {
                DeletePathForMessage(message);
            }

            return path[currentIndex + 1].Node;
        }

        public void DeletePathForMessage(Message message)
        {
            Paths.Remove(message);
        }
    }
}
