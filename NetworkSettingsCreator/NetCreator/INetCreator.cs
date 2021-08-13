using NetSim.Model;
using NetworkSettingsCreator.Configuration;

namespace NetworkSettingsCreator.NetCreator
{
	public interface INetCreator
	{
		NetworkSettings CreateNet(NetworkConfiguration configuration);
	}
}