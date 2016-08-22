using System;

namespace Venturer.Core.Input
{
	internal static class InputHandler
	{
		public static Command Translate(ConsoleKeyInfo key, InputContext inputContext)
		{
			switch (inputContext)
			{
				case InputContext.MainMenu:
					return TranslateForMainMenu(key);
				case InputContext.Game:
					return TranslateForGame(key);
				case InputContext.Menu:
					return TranslateForMenu(key);
			}

			return Command.Null;
		}

		private static Command TranslateForMainMenu(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				case ConsoleKey.W:
				case ConsoleKey.A:
				case ConsoleKey.UpArrow:
				case ConsoleKey.LeftArrow:
					return Command.MoveUp;
				case ConsoleKey.S:
				case ConsoleKey.D:
				case ConsoleKey.DownArrow:
				case ConsoleKey.RightArrow:
					return Command.MoveDown;
				case ConsoleKey.P:
				case ConsoleKey.Enter:
					return Command.Select;
				case ConsoleKey.Escape:
					return Command.Quit;
			}

			return Command.Null;
		}

		private static Command TranslateForMenu(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				case ConsoleKey.Escape:
					return Command.EscapeMenu;
				case ConsoleKey.D1:
				case ConsoleKey.NumPad1:
					return Command.MenuItem1;
				case ConsoleKey.D2:
				case ConsoleKey.NumPad2:
					return Command.MenuItem2;
				case ConsoleKey.D3:
				case ConsoleKey.NumPad3:
					return Command.MenuItem3;
				case ConsoleKey.D4:
				case ConsoleKey.NumPad4:
					return Command.MenuItem4;
				case ConsoleKey.D5:
				case ConsoleKey.NumPad5:
					return Command.MenuItem5;
				case ConsoleKey.D6:
				case ConsoleKey.NumPad6:
					return Command.MenuItem6;
				case ConsoleKey.D7:
				case ConsoleKey.NumPad7:
					return Command.MenuItem7;
				case ConsoleKey.D8:
				case ConsoleKey.NumPad8:
					return Command.MenuItem8;
				case ConsoleKey.D9:
				case ConsoleKey.NumPad9:
					return Command.MenuItem9;
			}

			return Command.Null;
		}

		private static Command TranslateForGame(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				case ConsoleKey.Escape:
					return Command.ShowMenu;
				case ConsoleKey.W:
					return Command.MoveUp;
				case ConsoleKey.A:
					return Command.MoveLeft;
				case ConsoleKey.S:
					return Command.MoveDown;
				case ConsoleKey.D:
					return Command.MoveRight;
				case ConsoleKey.M:
					return Command.Misc;
			}

			return Command.Null;
		}
	}
}
