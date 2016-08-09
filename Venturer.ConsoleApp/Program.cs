using System;
using System.IO;
using System.Text;
using Microsoft.Win32.SafeHandles;
using Venturer.Core;
using Venturer.Core.Output;

namespace Venturer.ConsoleApp
{
	public static class Program
	{
		[STAThread]
		public static void Main()
		{
			var h = NativeMethods.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
			if (h.IsInvalid)
			{
				Console.WriteLine("Error initializing.");
				Console.ReadKey();
				return;
			}

			using (var game = new Game())
			{
				Console.SetWindowSize(Game.WindowWidth, Game.WindowHeight);
				Console.SetBufferSize(Game.WindowWidth, Game.WindowHeight);
				Console.Title = game.Title;
				Console.OutputEncoding = Encoding.Unicode;
				Console.CursorVisible = false;

				//game.Draw += () => { Write(game.ToScreen(), h); };

				bool shouldQuit;
				do
				{
					Write(game.ToScreen(), h);
					shouldQuit = game.Input(Console.ReadKey(true));
				} while (!shouldQuit);
			}
		}

		private static void Write(Screen screen, SafeFileHandle h)
		{
			var buf = new CharInfo[Game.WindowWidth * Game.WindowHeight];
			var rect = new SmallRect { Left = 0, Top = 0, Right = Game.WindowWidth, Bottom = Game.WindowHeight };

			for (var y = 0; y < Game.WindowHeight; y++)
			{
				for (var x = 0; x < Game.WindowWidth; x++)
				{
					var b = y * Game.WindowWidth + x;
					buf[b].Attributes = AttributeAsShort(screen.Values[x, y].ForegroundColor, screen.Values[x, y].BackgroundColor);
					buf[b].Char.AsciiChar = (byte)screen.Values[x, y].Value;
				}
			}

			NativeMethods.WriteConsoleOutput(h, buf,
				new Coord { X = Game.WindowWidth, Y = Game.WindowHeight },
				new Coord { X = 0, Y = 0 },
				ref rect);
		}

		private static short AttributeAsShort(ConsoleColor foreground, ConsoleColor background)
		{
			return (short)((short)foreground + 16 * (short)background);
		}
	}
}
