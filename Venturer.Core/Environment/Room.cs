using System;
using System.Collections.Generic;
using Venturer.Core.Common;
using Venturer.Core.Environment.Tiles;
using Venturer.Core.Output;

namespace Venturer.Core.Environment
{
	public class Room
	{
		private readonly int _viewDistance;
		private readonly Tile[,] _tiles;

		public string Name { get; }
		public int Width { get; }
		public int Height { get; }
		public Coord StartingLocation;

		internal readonly List<Door> Doors;

		public Action OnEnter;
		public Action<Door> OnExit;
		public bool DoneExiting;
		public Door DoorExited;

		internal event EventHandler<ViewPort> ShowNewViewPort;

		public Room(string name, int width, int height, Tile[,] tiles, List<Door> doors)
		{
			Name = name;
			Doors = doors;
			_tiles = tiles;
			_viewDistance = 15;
			Width = width;
			Height = height;

			SetWallVisuals();
			SetFloorVisuals();
			SetDoors();
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

		private void SetWallVisuals()
		{
			for (var x = 0; x < Width; x++)
			{
				for (var y = 0; y < Height; y++)
				{
					var wallTile = _tiles[x, y] as WallTile;
					if (wallTile == null) continue;
					var top = y > 0 && _tiles[x, y - 1] is WallTile;
					var bottom = y < Height - 1 && _tiles[x, y + 1] is WallTile;
					var left = x > 0 && _tiles[x - 1, y] is WallTile;
					var right = x < Width - 1 && _tiles[x + 1, y] is WallTile;
					wallTile.SetNeighbors(top, right, bottom, left);
				}
			}
		}

		private void SetFloorVisuals()
		{
			var r = new Random();
			for (var x = 0; x < Width; x++)
			{
				for (var y = 0; y < Height; y++)
				{
					var floorTile = _tiles[x, y] as FloorTile;
					if (floorTile == null) continue;
					switch (r.Next(10))
					{
						case 0:
							floorTile.SetRepresentation(CodePoint.Comma);
							break;
						case 1:
							floorTile.SetRepresentation(CodePoint.Period);
							break;
						case 2:
							floorTile.SetRepresentation(CodePoint.Apostrophe);
							break;
					}
				}
			}
		}

		private void SetDoors()
		{
			foreach (var door in Doors)
			{
				_tiles[door.Location.X, door.Location.Y] = new DoorTile();
			}
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

		public void CreateNewViewPort(ViewPort viewPort)
		{
			ShowNewViewPort?.Invoke(this, viewPort);
		}
	}
}
