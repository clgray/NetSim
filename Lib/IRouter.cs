using System;
using System.Collections.Generic;
using System.Text;

namespace NetSim.Lib
{
    public interface IRouter
    {
        public INode GetRoute(INode currentNode, string targetId);
    }
}
