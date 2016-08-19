using System.Collections.Generic;
using Venturer.Core.Common;
using Venturer.Core.Screens;

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
			var doors = new List<Door>
			{
				new Door(new Coord(0, 3), "end", new Coord(28, 2))
			};
			var room = new Room(width, height, doors);
			room.OnEnter = () =>
			{
				room.CreateNewViewPort(new Menu("Room menu", new List<MenuOption>
				{
					new MenuOption("one", () => { }, false),
					new MenuOption("two", () => { }, false)
				}, () => { }));
			};
			return room;
		}
	}
}
