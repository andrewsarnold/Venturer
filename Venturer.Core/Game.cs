using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Venturer.Core.Input;
using Venturer.Core.Output;
using Venturer.Core.Screens;

namespace Venturer.Core
{
	public class Game : IDisposable
	{
		private readonly IGameData _gameData;
		private readonly Stack<ViewPort> _screenStack;
		private bool _shouldQuit;

		public string Title => _gameData.GameTitle;

		public event DrawHandler Draw;
		public delegate void DrawHandler();

		public event EventHandler<string> UpdateTitle;

		public Game(IGameData gameData)
		{
			_gameData = gameData;
			var timer = new Timer(83.3333);
			timer.Elapsed += (sender, args) =>
			{
				Draw?.Invoke();
			};
			timer.Start();

			_screenStack = new Stack<ViewPort>();
			_screenStack.Push(new MainMenu(_gameData));
		}

		/// <summary>
		///     Processes key input.
		/// </summary>
		/// <param name="key"></param>
		/// <returns>True when input sequence should end.</returns>
		public bool Input(ConsoleKeyInfo key)
		{
			// Everything should be handled by screens

			// Foreach screen in stack, top-down:
			//   Should I handle?
			//   Should I pass this on down the stack?
			foreach (var viewPort in _screenStack)
			{
				var shouldBubble = viewPort.HandleInput(InputHandler.Translate(key, viewPort.InputContext));

				// Find out if the screen wants us to quit
				_shouldQuit = viewPort.ShouldQuit;

				if (!shouldBubble)
				{
					break;
				}
			}

			// Write new messages / add new screens
			AddNewScreens();

			// Destroy expired screens
			DestroyOldScreens();

			// To be moved elsewhere
			UpdateTitle?.Invoke(this, _gameData.GameTitle);

			return _shouldQuit;
		}

		private void DestroyOldScreens()
		{
			var tempStack = new Stack<ViewPort>();
			while (_screenStack.Count > 0)
			{
				if (!_screenStack.Peek().ShouldDestroy)
				{
					tempStack.Push(_screenStack.Pop());
				}
				else
				{
					_screenStack.Pop();
				}
			}

			while (tempStack.Count > 0)
			{
				_screenStack.Push(tempStack.Pop());
			}
		}

		private void AddNewScreens()
		{
			var newScreens = _screenStack.Select(s => s.GetAndClearNewViewPort()).Where(v => v != null).ToList();
			foreach (var screen in newScreens)
			{
				_screenStack.Push(screen);
			}
		}

		public void Dispose()
		{
			// Save and clean up here
		}

		public Screen ToScreen()
		{
			var screens = CompileScreenList(_gameData.WindowWidth, _gameData.WindowHeight);
			return WriteScreens(screens);
		}

		private List<Screen> CompileScreenList(int width, int height)
		{
			return _screenStack.Select(s => s.ToScreen(width, height)).Reverse().ToList();
		}

		private Screen WriteScreens(IReadOnlyCollection<Screen> screens)
		{
			var returnCh = new Glyph[_gameData.WindowWidth, _gameData.WindowHeight];

			for (var x = 0; x < _gameData.WindowWidth; x++)
			{
				for (var y = 0; y < _gameData.WindowHeight; y++)
				{
					foreach (var screen in screens)
					{
						if (x < screen.Width && y < screen.Height && screen.Values[x, y].Value != '\0')
						{
							returnCh[x + screen.XOffset, y + screen.YOffset] = screen.Values[x, y];
						}
					}
				}
			}

			return new Screen(returnCh);
		}
	}
}
