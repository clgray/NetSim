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
		private readonly Dictionary<string, IRouter> _routers;

		public RouterProvider(NetworkSettings settings)
		{
			_settings = settings; // TBD: routing settings
			_routers = new Dictionary<string, IRouter>()
			{
				{ "dijkstrapath", new DijkstraRouterByShortestPath() },
				{ "dijkstraf6", new DijkstraRouterF6() },
				{ "dijkstraqueue", new DijkstraRouterByShortestQueue() },
				{ "composite", new СompositeDijkstraRouterF6() },
				{ "dfs", new DFSRouter() },
				{ "allnet8", new AllNetDijkstraRouterF8() },
				{ "allnet3", new AllNetDijkstraRouterF3() }
			};
		}

		public IRouter GetRouter(string routerName)
		{
			return _routers[routerName.ToLower()];
		}
	}
}