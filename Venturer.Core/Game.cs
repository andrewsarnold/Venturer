﻿using System;
using System.Timers;

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

		public void Dispose()
		{
			// Save and clean up here
		}
	}
}
