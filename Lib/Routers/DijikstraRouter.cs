using System;
using System.Collections.Generic;
using System.Text;

namespace NetSim.Lib.Routers
{
    public class DijikstraRouter : IRouter
    {
        public DijikstraRouter()
        {
            // TODO: get network graph
        }

        public INode GetRoute(INode currentNode, string targetId)
        {
            throw new NotImplementedException();
        }
    }
}
