using System;
using Venturer.Core.Common;
using Venturer.Core.Environment;
using Venturer.Core.Environment.Tiles;
using Venturer.Core.Mobs;
using Venturer.Core.Output;

namespace Venturer.Core.Screens
{
	internal class GameScreen : ViewPort
	{
		private const int MaxCameraDistance = 5;
		private Room _room;
		private Coord _camera;
		private ViewPort _newScreen;
		private readonly Player _player;

		public bool ShouldQuit { get; private set; }
		public bool ShouldReset { get; private set; }

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
			_player = new Player(new Coord(_room.Width / 2, _room.Height / 2));
			_camera = _player.Position;
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
					DirectionallyInteract(key);
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
			_room.Draw(chars, roomLeft, roomTop, _player.Position);
			_player.Draw(chars,
				roomLeft + _player.Position.X,
				roomTop + _player.Position.Y,
				_room.BackgroundColorAt(_player.Position));
			return new Screen(chars, xOffset, yOffset);
		}

		internal override ViewPort GetAndClearNewViewPort()
		{
			var newScreen = _newScreen;
			_newScreen = null;
			return newScreen;
		}

		private void DirectionallyInteract(ConsoleKeyInfo key)
		{
			var target = TargetSpace(key);
			TryToMove(target, key.Key == ConsoleKey.A || key.Key == ConsoleKey.D);
		}

		private Coord TargetSpace(ConsoleKeyInfo key)
		{
			var targetX = _player.Position.X;
			var targetY = _player.Position.Y;
			switch (key.Key)
			{
				case ConsoleKey.W:
					targetY--;
					break;
				case ConsoleKey.A:
					targetX--;
					break;
				case ConsoleKey.S:
					targetY++;
					break;
				case ConsoleKey.D:
					targetX++;
					break;
			}

			return new Coord(targetX, targetY);
		}

		private Direction TryToMove(Coord target, bool movedHorizontally)
		{
			var couldMove = _room.IsInRoom(target) && _room.IsFreeOfArchitecture(target);
			if (couldMove)
			{
				_player.Position = target;
				ShiftCamera(movedHorizontally);
				_room.SetAsSeen(target);
			}

			if (target.X < _player.Position.X) return Direction.West;
			if (target.Y < _player.Position.Y) return Direction.North;
			if (target.X > _player.Position.X) return Direction.East;
			if (target.Y > _player.Position.Y) return Direction.South;
			return Direction.None;
		}

		private void ShiftCamera(bool movedHorizontally)
		{
			var distance = Coord.Distance(_camera, _player.Position);
			if (distance < MaxCameraDistance)
			{
				return;
			}

			_camera = movedHorizontally
				? (_camera.X < _player.Position.X
					? new Coord(_camera.X + 1, _camera.Y)
					: new Coord(_camera.X - 1, _camera.Y))
				: (_camera.Y < _player.Position.Y
					? new Coord(_camera.X, _camera.Y + 1)
					: new Coord(_camera.X, _camera.Y - 1));
		}

		internal override bool ShouldDestroy
		{
			get { return false; }
		}
	}
}
