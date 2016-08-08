using System;
using System.Timers;
using Venturer.Core.Output;
using Venturer.Core.Screens;

namespace Venturer.Core
{
	public class Game : IDisposable
	{
		public const int WindowWidth = 78;
		public const int WindowHeight = 24;
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
			return new TestScreen().ToScreen(WindowWidth, WindowHeight);
		}
	}
}
