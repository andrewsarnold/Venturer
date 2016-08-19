using System.Collections.Generic;

namespace Venturer.Core.Environment
{
	internal class Level
	{
		internal Dictionary<string, Room> Rooms;

		public Level(Dictionary<string, Room> rooms)
		{
			Rooms = rooms;
		}
	}
}
