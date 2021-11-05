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
        private readonly IRouter _dijkstra;
        private readonly IRouter _dfs;
        public RouterProvider(NetworkSettings settings)
        {
            _settings = settings; // TBD: routing settings
            _dijkstra = new Dijkstra();
            _dfs = new DFSRouter();
        }

        public IRouter GetRouter(string routerName)
        {
            return routerName.ToLower() switch
            {
                "dijkstra" => _dijkstra,
                "dfs" => _dfs,
                _ => throw new NotImplementedException()
            };
        }
    }
}
