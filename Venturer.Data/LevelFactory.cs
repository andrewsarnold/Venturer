﻿using System.Collections.Generic;
using Venturer.Core.Common;
using Venturer.Core.Environment;
using Venturer.Core.Screens;

namespace Venturer.Data
{
	internal class LevelFactory : ILevelFactory
	{
		public Level GetLevel()
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
			room.OnExit = () =>
			{
				room.CreateNewViewPort(new MultiTextScreen("you left the room"));
			};
			return room;
		}
	}
}