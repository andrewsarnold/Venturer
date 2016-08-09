using System;

namespace Venturer.Core.Input
{
	internal static class InputHandler
	{
		public static Command Translate(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				case ConsoleKey.Escape:
					return Command.Quit;
				case ConsoleKey.W:
					return Command.MoveUp;
				case ConsoleKey.A:
					return Command.MoveLeft;
				case ConsoleKey.S:
					return Command.MoveDown;
				case ConsoleKey.D:
					return Command.MoveRight;
				default:
					return Command.Quit;
			}
		}
	}
}
