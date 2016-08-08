using System;
using Venturer.Core.Common;
using Venturer.Core.Environment;
using Venturer.Core.Output;

namespace Venturer.Core.Screens
{
	internal class GameScreen : ViewPort
	{
		private Room _room;
		private Coord _camera;
		private ViewPort _newScreen;

		public GameScreen()
		{
			var roomWidth = 20;
			var roomHeight = 20;

			var tiles = new Tile[roomWidth, roomHeight];
			for (var y = 0; y < roomHeight; y++)
			{
				for (var x = 0; x < roomWidth; x++)
				{
					tiles[x, y] = new FloorTile();
				}
			}
			_room = new Room(tiles, roomWidth, roomHeight);
			_camera = new Coord(roomWidth / 2, roomHeight / 2);
		}

		internal override bool HandleInput(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				case ConsoleKey.Escape:
					// menu
					break;
				case ConsoleKey.W:
				case ConsoleKey.A:
				case ConsoleKey.S:
				case ConsoleKey.D:
					//DirectionallyInteract(key);
					break;
			}

			// GameScreen should be the last thing in the stack.
			return false;
		}

		internal override Screen ToScreen(int width, int height, int xOffset = 0, int yOffset = 0)
		{
			var chars = new Glyph[width, height];
			var roomLeft = width / 2 - _camera.X;
			var roomTop = height / 2 - _camera.Y;
			_room.Draw(chars, roomLeft, roomTop, new Coord(5, 5));
			//_player.Draw(chars,
			//	roomLeft + _player.Position.X,
			//	roomTop + _player.Position.Y,
			//	_room.BackgroundColorAt(ToRoomCoordinates(_player.Position)));
			return new Screen(chars, xOffset, yOffset);
		}

		internal override ViewPort NewViewPort
		{
			get { return null; }
		}

		internal override bool ShouldDestroy
		{
			get { return false; }
		}
	}
}
