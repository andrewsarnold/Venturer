using System;

namespace Venturer.Core
{
	public class Game : IDisposable
	{
		public const int WindowWidth = 78;
		public const int WindowHeight = 24;
		public string Title { get { return "Venturer.Core"; } }

		public void Dispose()
		{
			// Save and clean up here
		}
	}
}
