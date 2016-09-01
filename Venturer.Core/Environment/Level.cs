using System.Collections.Generic;

namespace Venturer.Core.Environment
{
	public class Level
	{
		internal readonly Dictionary<string, Room> Rooms;
		internal readonly Room FirstRoom;

		public Level(Dictionary<string, Room> rooms, string firstRoom)
		{
			Rooms = rooms;
			FirstRoom = rooms[firstRoom];
		}
	}
}
