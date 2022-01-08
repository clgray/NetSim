using System;
using System.Collections.Generic;
using System.Text;
using NetSim.Lib;
using NetSim.Lib.Routers;
using NetSim.Model;

namespace NetSim.Providers
{
    public class RouterProvider
    {
        private readonly NetworkSettings _settings;
        private readonly IRouter _dijkstraPath;
        private readonly IRouter _dfs;
        private DijkstraRouterF6 _dijkstraF6;
        private DijkstraRouterByShortestQueue _dijkstraQueue;

        public RouterProvider(NetworkSettings settings)
        {
            _settings = settings; // TBD: routing settings
            _dijkstraF6 = new DijkstraRouterF6();
            _dijkstraPath = new DijkstraRouterByShortestPath();
            _dijkstraQueue = new DijkstraRouterByShortestQueue();
            _dfs = new DFSRouter();
        }

        public IRouter GetRouter(string routerName)
        {
            return routerName.ToLower() switch
            {
                "dijkstrapath" => _dijkstraPath,
                "dijkstraf6" => _dijkstraF6,
                "dijkstraqueue" => _dijkstraQueue,
                "dfs" => _dfs,
                _ => throw new NotImplementedException()
            };
        }
    }
}
