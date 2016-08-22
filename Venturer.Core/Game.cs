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
		public const int WindowWidth = 78;
		public const int WindowHeight = 24;
		private readonly Stack<ViewPort> _screenStack;
		private bool _shouldQuit;

		public string Title => _gameData.GameTitle;

		public event DrawHandler Draw;
		public delegate void DrawHandler();

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
			_screenStack.Push(new GameScreen(gameData, WindowWidth, WindowHeight));
		}

		/// <summary>
		///     Processes key input.
		/// </summary>
		/// <param name="key"></param>
		/// <returns>True when input sequence should end.</returns>
		public bool Input(ConsoleKeyInfo key)
		{
			// Everything should be handled by screens
			var shouldReset = false;

			// Foreach screen in stack, top-down:
			//   Should I handle?
			//   Should I pass this on down the stack?
			foreach (var viewPort in _screenStack)
			{
				var shouldBubble = viewPort.HandleInput(InputHandler.Translate(key, viewPort.InputContext));

				// Find out if the game screen wants us to quit
				var gameScreen = viewPort as GameScreen;
				if (gameScreen != null)
				{
					_shouldQuit = gameScreen.ShouldQuit;
					if (gameScreen.ShouldReset)
					{
						shouldReset = true;
						break;
					}
				}

				if (!shouldBubble)
				{
					break;
				}
			}

			if (shouldReset)
			{
				Initialize();
			}

			// Write new messages / add new screens
			AddNewScreens();

			// Destroy expired screens
			DestroyOldScreens();

			return _shouldQuit;
		}

		private void DestroyOldScreens()
		{
			var top = _screenStack.Peek();
			do
			{
				if (top.ShouldDestroy)
				{
					_screenStack.Pop();
					top = _screenStack.Peek();
				}
			} while (top.ShouldDestroy);
		}

		private void AddNewScreens()
		{
			var newScreens = _screenStack.Select(s => s.GetAndClearNewViewPort()).Where(v => v != null).ToList();
			foreach (var screen in newScreens)
			{
				_screenStack.Push(screen);
			}
		}

		private void Initialize()
		{
			_screenStack.Clear();
			_screenStack.Push(new GameScreen(_gameData, WindowWidth, WindowHeight));
		}

		public void Dispose()
		{
			// Save and clean up here
		}

		public Screen ToScreen()
		{
			var screens = CompileScreenList(WindowWidth, WindowHeight);
			return WriteScreens(screens, WindowWidth, WindowHeight);
		}

		private List<Screen> CompileScreenList(int width, int height)
		{
			return _screenStack.Select(s => s.ToScreen(width, height)).Reverse().ToList();
		}

		private static Screen WriteScreens(List<Screen> screens, int width, int height)
		{
			var returnCh = new Glyph[width, height];

			for (var x = 0; x < width; x++)
			{
				for (var y = 0; y < height; y++)
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
