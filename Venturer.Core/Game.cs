using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Venturer.Core.Output;
using Venturer.Core.Screens;

namespace Venturer.Core
{
	public class Game : IDisposable
	{
		public const int WindowWidth = 78;
		public const int WindowHeight = 24;
		private readonly Stack<ViewPort> _screenStack;

		public string Title { get { return "Venturer.Core"; } }

		public event DrawHandler Draw;
		public delegate void DrawHandler();

		public Game()
		{
			var timer = new Timer(83.3333);
			timer.Elapsed += (sender, args) =>
			{
				if (Draw != null)
				{
					Draw();
				}
			};
			timer.Start();

			_screenStack = new Stack<ViewPort>();
			_screenStack.Push(new GameScreen());
		}

		/// <summary>
		///     Processes key input.
		/// </summary>
		/// <param name="key"></param>
		/// <returns>True when input sequence should end.</returns>
		public bool Input(ConsoleKeyInfo key)
		{
			return key.Key == ConsoleKey.Escape;
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
						if (x < screen.Width && y < screen.Height && screen.Values[x, y] != null && screen.Values[x, y].Value != '\0')
						{
							returnCh[x + screen.XOffset, y + screen.YOffset] = screen.Values[x, y];
						}
						else
						{
							returnCh[x + screen.XOffset, y + screen.YOffset] = new Glyph(' ');
						}
					}
				}
			}

			return new Screen(returnCh);
		}
	}
}
