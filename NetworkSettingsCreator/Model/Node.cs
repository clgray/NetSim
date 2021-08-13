using System.Collections.Generic;

namespace NetworkSettingsCreator.Model
{
	public class Node
	{
		public Node(int id)
		{
			Id = id;
			Connections = new List<Connection>();
		}

		public List<Connection> Connections { get; }

		public int Id { get; }
	}
}