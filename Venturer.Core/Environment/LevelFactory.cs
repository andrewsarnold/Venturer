using System.Collections.Generic;
using Venturer.Core.Common;
using Venturer.Core.Screens;

namespace Venturer.Core.Environment
{
	internal static class LevelFactory
	{
		internal static Level GetLevel(int screenWidth, int screenHeight)
		{
			return new Level(new Dictionary<string, Room>
			{
				{ "start", MakeRoom(20, 10, screenWidth, screenHeight) },
				{ "end", MakeRoom(30, 25, screenWidth, screenHeight) }
			});
		}

		private static Room MakeRoom(int width, int height, int screenWidth, int screenHeight)
		{
			var doors = new List<Door>
			{
				new Door(new Coord(0, 3), "end", new Coord(28, 2))
			};
			var room = new Room(width, height, doors);
			room.OnEnter = () =>
			{
				room.CreateNewViewPort(new Menu(screenWidth, screenHeight, "Room menu", new List<MenuOption>
				{
					new MenuOption("one", () => { }, false),
					new MenuOption("two", () => { }, false)
				}, () => { }));
			};
			return room;
		}
	}
}
