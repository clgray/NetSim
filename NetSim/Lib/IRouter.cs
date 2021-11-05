using System;
using System.Collections.Generic;
using System.Text;
using NetSim.Model.Message;

namespace NetSim.Lib
{
    public interface IRouter
    {
        public INode GetRoute(INode currentNode, string targetId, Message message);
        void RebuildRoutes();
    }
}
