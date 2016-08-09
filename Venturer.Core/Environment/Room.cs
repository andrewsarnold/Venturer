using System;
using Venturer.Core.Common;
using Venturer.Core.Environment.Tiles;
using Venturer.Core.Output;

namespace Venturer.Core.Environment
{
	public class Room
	{
		private readonly int _viewDistance;
		private readonly Tile[,] _tiles;

		public int Width { get; private set; }
		public int Height { get; private set; }

		internal Room(Tile[,] tiles, int width, int height)
		{
			_tiles = tiles;
			_viewDistance = 15;
			Width = width;
			Height = height;

			var r = new Random();
			for (var x = 0; x < width; x++)
			{
				for (var y = 0; y < height; y++)
				{
					_tiles[x, y] = r.NextDouble() < 0.8
						? new FloorTile()
						: (Tile)new WallTile();
				}
			}
		}

		public void Draw(Glyph[,] chars, int roomLeft, int roomTop, Coord player)
		{
			var isLit = FindLitTiles(player);
			DrawInterior(chars, roomLeft, roomTop, isLit);
		}

		internal bool IsFreeOfArchitecture(Coord target)
		{
			return target.X >= 0 && target.Y >= 0 && target.X < Width && target.Y < Height
				   && _tiles[target.X, target.Y].CanTraverse;
		}

		internal bool IsInRoom(Coord target)
		{
			return
				target.X >= 0 &&
				target.Y >= 0 &&
				target.X < Width &&
				target.Y < Height;
		}

		private bool[,] FindLitTiles(Coord player)
		{
			var isLit = new bool[Width, Height];
			Shadowcaster.ComputeFieldOfViewWithShadowCasting(player.X, player.Y, _viewDistance,
				(x, y) => x < 0 || x >= Width || y < 0 || y >= Height || _tiles[x, y] is WallTile,
				(x, y) =>
				{
					if (x >= 0 && x < Width && y >= 0 && y < Height)
					{
						_tiles[x, y].SetAsSeen();
						isLit[x, y] = true;
					}
				});
			return isLit;
		}

		private void DrawInterior(Glyph[,] chars, int roomLeft, int roomTop, bool[,] isLit)
		{
			for (var x = 0; x < Width; x++)
			{
				for (var y = 0; y < Height; y++)
				{
					Screen.AddChar(chars, x + roomLeft, y + roomTop, _tiles[x, y].ToCharacter(isLit[x, y]));
				}
			}
		}

		internal void SetAsSeen(Coord target)
		{
			_tiles[target.X, target.Y].SetAsSeen();
		}

		internal ConsoleColor BackgroundColorAt(Coord c)
		{
			return _tiles[c.X, c.Y].ToCharacter(true).BackgroundColor;
		}
	}
}
