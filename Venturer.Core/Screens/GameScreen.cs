using System.Collections.Generic;
using System.Linq;
using Venturer.Core.Common;
using Venturer.Core.Environment;
using Venturer.Core.Input;
using Venturer.Core.Mobs;
using Venturer.Core.Output;

namespace Venturer.Core.Screens
{
	internal class GameScreen : ViewPort
	{
		private readonly IGameData _gameData;
		private const int MaxCameraDistance = 5;
		private Room _room;
		private Coord _camera;
		private ViewPort _newScreen;
		private readonly Level _level;
		private readonly Player _player;
		private bool _shouldDestroy;

		internal override bool ShouldDestroy => _shouldDestroy;
		internal override InputContext InputContext => InputContext.Game;
		internal override bool ShouldQuit => false;

		public GameScreen(IGameData gameData, int width, int height)
			: base(width, height)
		{
			_gameData = gameData;
			_player = new Player(new Coord());
			_level = _gameData.LevelFactory.GetLevel();
			SetUpRoom(_level.Rooms.First().Value);
		}

		internal override bool HandleInput(Command command)
		{
			switch (command)
			{
				case Command.MoveUp:
				case Command.MoveDown:
				case Command.MoveLeft:
				case Command.MoveRight:
					DirectionallyInteract(command);
					break;
				case Command.ShowMenu:
					_newScreen = PauseMenu;
					break;
				case Command.Misc:
					_newScreen = new MultiTextScreen(new List<string>
					{
						"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
						"Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
						"Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
						"Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
					});
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

		private void SetUpRoom(Room room)
		{
			SetUpRoom(room, new Coord(room.Width / 2, room.Height / 2));
		}

		private void SetUpRoom(Room room, Coord playerLocation)
		{
			_room?.OnExit?.Invoke();
			_room = room;
			_player.Position = playerLocation;
			_camera = _player.Position;
			_room.ShowNewViewPort += (sender, port) =>
			{
				_newScreen = port;
			};
			_room.OnEnter?.Invoke();
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
			// Is there a door here?
			var doorHere = _room.Doors.SingleOrDefault(d => d.Location.Equals(target));
			if (doorHere != null)
			{
				SetUpRoom(_level.Rooms[doorHere.TargetRoom], doorHere.TargetCoord);
				return Direction.None;
			}

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

		private Menu PauseMenu
		{
			get
			{
				return new Menu("P A U S E D", new List<MenuOption>
				{
					new MenuOption("Continue", () => { }, false, true),
					new MenuOption("Save", () =>
					{
					    _newScreen = CommonMenus.SaveSlotPicker("Save game", SaveGame, () => { });
					}, false, false),
					new MenuOption("Quit", () =>
					{
						_newScreen = new MainMenu(_gameData);
						_shouldDestroy = true;
					}, true, true)
				},
				() => { });
			}
		}

	    private void SaveGame(int saveSlot)
	    {
	        _gameData.SaveGame(saveSlot);
            _newScreen = new MultiTextScreen("Game saved to slot " + saveSlot);
	    }
	}
}
