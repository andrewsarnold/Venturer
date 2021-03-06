﻿using System;
using System.Collections.Generic;
using Venturer.Core.Common;
using Venturer.Core.Input;
using Venturer.Core.Output;

namespace Venturer.Core.Screens
{
    internal class MainMenu : ViewPort
	{
		private readonly IGameData _gameData;
		private readonly List<Tuple<string, Action>> _options;
		private ViewPort _newScreen;
		private int _selectedIndex;
		private bool _shouldDestroy;
		private bool _shouldQuit;

		public MainMenu(IGameData gameData)
			: base(gameData.WindowWidth, gameData.WindowHeight)
		{
			_gameData = gameData;
			_options = new List<Tuple<string, Action>>
			{
				new Tuple<string, Action>("New game", () =>
				{
					_newScreen = new GameScreen(_gameData, Width, Height);
					_shouldDestroy = true;
				}),
				new Tuple<string, Action>("Continue", () => { }),
				new Tuple<string, Action>("Load", () => { _newScreen = CommonMenus.SaveSlotPicker("Load game", LoadGame, () => { }); }),
				new Tuple<string, Action>("Quit", () => { _shouldQuit = true; })
			};
			_selectedIndex = 0;
		}

		internal override bool HandleInput(Command command)
		{
			switch (command)
			{
				case Command.MoveUp:
					_selectedIndex = (_selectedIndex + _options.Count - 1) % _options.Count;
					break;
				case Command.MoveDown:
					_selectedIndex = (_selectedIndex + 1) % _options.Count;
					break;
				case Command.Select:
					_options[_selectedIndex].Item2();
					break;
				case Command.Quit:
					_shouldQuit = true;
					break;
			}
			
			return false;
		}

		internal override Screen ToScreen(int width, int height, int xOffset = 0, int yOffset = 0)
		{
			var glyphs = new Glyph[width, height];

			Utilities.WriteTextLine(glyphs, new Coord(4, 2), Utilities.Stylize(_gameData.GameTitle));

			var startY = height / 2 - _options.Count / 2;
			for (var y = 0; y < _options.Count; y++)
			{
				Utilities.WriteTextLine(glyphs, new Coord(5, startY + y), string.Format("{0} {1}",
					y == _selectedIndex ? ">" : " ",
					_options[y].Item1));
			}

			return new Screen(glyphs);
		}

		internal override ViewPort GetAndClearNewViewPort()
		{
			return _newScreen;
		}

		internal override bool ShouldDestroy => _shouldDestroy;
		internal override InputContext InputContext => InputContext.MainMenu;
		internal override bool ShouldQuit => _shouldQuit;

		private void LoadGame(int slotId)
		{
			_gameData.LoadGame(slotId);
			_newScreen = new GameScreen(_gameData, Width, Height);
			_shouldDestroy = true;
		}
	}
}
