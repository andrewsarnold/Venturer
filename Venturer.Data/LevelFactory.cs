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
				{ "start", MakeRoom(200, 30) }
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
				StartingLocation = new Coord(5, 5)
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
