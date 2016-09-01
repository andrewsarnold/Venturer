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
			SetUpRoom(_level.FirstRoom);
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
					return false;
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
				case Command.Inspect:
					Inspect();
					break;
				case Command.Inventory:
					Inventory();
					break;
			}

			if (_room.DoneExiting)
			{
				SetUpRoom(_level.Rooms[_room.DoorExited.TargetRoom], _room.DoorExited.TargetCoord);
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
				_room.BackgroundColorAt(_player.Position, true));
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
			SetUpRoom(room, room.StartingLocation);
		}

		private void SetUpRoom(Room room, Coord playerLocation)
		{
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

		private void TryToMove(Coord target, bool movedHorizontally)
		{
			// Is there a door here?
			var doorHere = _room.Doors.SingleOrDefault(d => d.Location.Equals(target));
			if (doorHere != null)
			{
				if (_room.OnExit != null)
				{
					_room.OnExit(doorHere);
				}
				else
				{
					SetUpRoom(_level.Rooms[doorHere.TargetRoom], doorHere.TargetCoord);
				}
				return;
			}

			var couldMove = _room.IsInRoom(target) && _room.IsFreeOfArchitecture(target);
			if (!couldMove) return;
			_player.Position = target;
			ShiftCamera(movedHorizontally);
			_room.SetAsSeen(target);
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
				return new Menu(Utilities.Stylize("Paused"), new List<MenuOption>
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

		private void Inspect()
		{
			var itemsAroundPlayer = _room.Items.Where(i => Coord.Distance(i.Location, _player.Position) < 2).ToList();
			if (itemsAroundPlayer.Count > 0)
			{
				var options = itemsAroundPlayer.Select(i => new MenuOption(i.AsListItem, () =>
				{
					var innerOptions = i is CollectibleItem
						? new List<MenuOption>
						{
							new MenuOption("Take", () =>
							{
								_newScreen = new MultiTextScreen($"You take the {i.Name}.");
								_gameData.Inventory.Add((CollectibleItem)i);
								_room.Items.Remove(i);
							}, false, true),
							new MenuOption("Cancel", () => { }, false, true)
						}
						: new List<MenuOption>
						{
							new MenuOption("Cancel", () => { }, false, true)
						};

					_newScreen = new Menu(i.Name, innerOptions, () => { });
				}, false, true)).ToList();
				options.Add(new MenuOption("Cancel", () => { }, false, true));
				_newScreen = new Menu("You look around and see:", options, () => { });
			}
			else
			{
				_newScreen = new MultiTextScreen("You look around, but there is nothing nearby.");
			}
		}

		private void Inventory()
		{
			_newScreen = new Menu("Inventory", _gameData.Inventory.Select(i => new MenuOption(i.AsListItem, () => { }, false, false)).Union(new List<MenuOption>
			{
				new MenuOption("Cancel", () => { }, false, true)
			}).ToList(), () => { });
		}
	}
}
