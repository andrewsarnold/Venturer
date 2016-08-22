using System.Collections.Generic;

namespace Venturer.Core.Environment
{
	public class Level
	{
		internal Dictionary<string, Room> Rooms;

		public Level(Dictionary<string, Room> rooms)
		{
			Rooms = rooms;
		}
	}
}
