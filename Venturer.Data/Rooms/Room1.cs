using System.Collections.Generic;
using Venturer.Core.Common;
using Venturer.Core.Environment;
using Venturer.Core.Environment.Tiles;
using Venturer.Core.Screens;

namespace Venturer.Data.Rooms
{
	internal class Room1 : IRoomFactory
	{
		public Room MakeRoom()
		{
			const int width = 30;
			const int height = 30;
			var doors = new List<Door>
			{
				new Door(new Coord(29, 5), "end", new Coord(1, 2))
			};
			var room = new Room("start", width, height, SetWalls(width, height), doors)
			{
				StartingLocation = new Coord(5, 5)
			};
			room.OnExit = door =>
			{
				room.CreateNewViewPort(new MultiTextScreen("you left room 1"));
				room.DoneExiting = true;
				room.DoorExited = door;
			};
			return room;
		}

		private static Tile[,] SetWalls(int width, int height)
		{
			var tiles = new Tile[width, height];
			for (var x = 0; x < width; x++)
			{
				for (var y = 0; y < height; y++)
				{
					var tileSet = false;

					if (y > 4 && y < 6 && x > 5)
					{
						tiles[x, y] = new FloorTile();
						tileSet = true;
					}

					if (y == 4 || y == 6 && x > 5)
					{
						tiles[x, y] = new WallTile();
						tileSet = true;
					}

					var distToStart = Coord.Distance(new Coord(x, y), new Coord(5, 5));
					if (distToStart < 4)
					{
						tiles[x, y] = new FloorTile();
						tileSet = true;
					}

					if (distToStart <= 5 && !tileSet)
					{
						tiles[x, y] = new WallTile();
						tileSet = true;
					}

					if (!tileSet)
					{
						tiles[x, y] = new EmptyTile();
					}
				}
			}

			return tiles;
		}
	}
}
