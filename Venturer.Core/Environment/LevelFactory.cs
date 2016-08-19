using System.Collections.Generic;

namespace Venturer.Core.Environment
{
	internal static class LevelFactory
	{
		internal static Level GetLevel()
		{
			return new Level(new Dictionary<string, Room>
			{
				{ "start", MakeRoom(20, 10) },
				{ "end", MakeRoom(30, 25) }
			});
		}

		private static Room MakeRoom(int width, int height)
		{
			return new Room(width, height);
		}
	}
}
