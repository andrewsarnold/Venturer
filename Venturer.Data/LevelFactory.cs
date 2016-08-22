using System.Collections.Generic;
using Venturer.Core.Common;
using Venturer.Core.Environment;
using Venturer.Core.Environment.Tiles;
using Venturer.Core.Screens;

namespace Venturer.Data
{
	internal class LevelFactory : ILevelFactory
	{
		public Level GetLevel()
		{
			return new Level(new Dictionary<string, Room>
			{
				{ "start", MakeRoom(200, 10) }
			});
		}

		private static Room MakeRoom(int width, int height)
		{
			var doors = new List<Door>
			{
				new Door(new Coord(199, 5), null, new Coord(0, 0))
			};
			var room = new Room(width, height, SetWalls(width, height), doors)
			{
				StartingLocation = new Coord(2, 5)
			};
			room.OnExit = () =>
			{
				room.CreateNewViewPort(new MultiTextScreen("you left the room"));
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
					tiles[x, y] =
						x == 0 ||
						y == 0 ||
						x == width - 1 ||
						y == height - 1
							? new WallTile()
							: (Tile)new FloorTile();
				}
			}

			return tiles;
		}
	}
}
