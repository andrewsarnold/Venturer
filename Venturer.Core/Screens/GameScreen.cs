using System.Collections.Generic;
using Venturer.Core.Common;
using Venturer.Core.Environment;
using Venturer.Core.Environment.Tiles;
using Venturer.Core.Input;
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

		public GameScreen(int width, int height, int offsetX, int offsetY)
			: base(width, height, offsetX, offsetY)
		{
			var roomWidth = 50;
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

		internal override bool HandleInput(Command command)
		{
			switch (command)
			{
				case Command.Quit:
					// menu
					break;
				case Command.MoveUp:
				case Command.MoveDown:
				case Command.MoveLeft:
				case Command.MoveRight:
					DirectionallyInteract(command);
					break;
				case Command.ShowMenu:
					_newScreen = PauseMenu;
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

		private void DirectionallyInteract(Command command)
		{
			var target = TargetSpace(command);
			TryToMove(target, command == Command.MoveLeft || command == Command.MoveRight);
		}

		private Coord TargetSpace(Command command)
		{
			var targetX = _player.Position.X;
			var targetY = _player.Position.Y;
			switch (command)
			{
				case Command.MoveUp:
					targetY--;
					break;
				case Command.MoveLeft:
					targetX--;
					break;
				case Command.MoveDown:
					targetY++;
					break;
				case Command.MoveRight:
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

		internal override InputContext InputContext
		{
			get { return InputContext.Game; }
		}

		private Menu PauseMenu
		{
			get
			{
				return new Menu(Width, Height, "P A U S E D", new List<MenuOption>
				{
					new MenuOption("Continue", () => { }, false),
					new MenuOption("Reset", () => { ShouldReset = true; }, true),
					new MenuOption("Save", () => { }, false),
					new MenuOption("Quit", () => { ShouldQuit = true; }, true)
				},
				() => { });
			}
		}
	}
}
