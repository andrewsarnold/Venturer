using System.Collections.Generic;
using Venturer.Core;
using Venturer.Core.Common;
using Venturer.Core.Environment;
using Venturer.Core.Environment.Tiles;
using Venturer.Core.Screens;

namespace Venturer.Data.Rooms
{
	internal class Room2 : IRoomFactory
	{
		public Room MakeRoom()
		{
			const int width = 18;
			const int height = 8;
			var doors = new List<Door>
			{
				new Door(new Coord(0, 2), "start", new Coord(28, 5))
			};
			var room = new Room("end", width, height, SetWalls(width, height), doors, new List<Item>());
			room.OnEnter = () =>
			{
				room.CreateNewViewPort(new MultiTextScreen("you entered room 2"));
			};
			return room;
		}

		private Tile[,] SetWalls(int width, int height)
		{
			var tiles = new Tile[width, height];
			for (var x = 0; x < width; x++)
			{
				for (var y = 0; y < height; y++)
				{
					tiles[x, y] =
						x == 0 ||
						y == 0 ||
						x == width - 1 ||
						y == height - 1
							? (Tile)new WallTile()
							: new FloorTile();
				}
			}
			return tiles;
		}
	}
}
