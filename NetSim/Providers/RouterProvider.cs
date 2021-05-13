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
        private readonly DijkstraRouter _dijkstra;
        private readonly BFSRouter _bfs;
        public RouterProvider(NetworkSettings settings)
        {
            _settings = settings; // TBD: routing settings
            _dijkstra = new DijkstraRouter();
            _bfs = new BFSRouter();
        }

        public IRouter GetRouter(string routerName)
        {
            return routerName.ToLower() switch
            {
                "dijkstra" => _dijkstra,
                "bfs" => _bfs,
                _ => throw new NotImplementedException()
            };
        }
    }
}
