using System;
using System.IO;
using System.Text;
using Microsoft.Win32.SafeHandles;
using Venturer.Core;
using Venturer.Core.Output;
using Venturer.Data;

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

			var gameData = new GameData();
			using (var game = new Game(gameData))
			{
				Console.SetWindowSize(gameData.WindowWidth, gameData.WindowHeight);
				Console.SetBufferSize(gameData.WindowWidth, gameData.WindowHeight);
				Console.Title = game.Title;
				Console.OutputEncoding = Encoding.Unicode;
				Console.CursorVisible = false;

				//game.Draw += () => { Write(game.ToScreen(), h); };

				bool shouldQuit;
				do
				{
					Write(gameData, game.ToScreen(), h);
					shouldQuit = game.Input(Console.ReadKey(true));
				} while (!shouldQuit);
			}
		}

		private static void Write(IGameData gameData, Screen screen, SafeFileHandle h)
		{
			var buf = new CharInfo[gameData.WindowWidth * gameData.WindowHeight];
			var rect = new SmallRect { Left = 0, Top = 0, Right = (short)gameData.WindowWidth, Bottom = (short)gameData.WindowHeight };

			for (var y = 0; y < gameData.WindowHeight; y++)
			{
				for (var x = 0; x < gameData.WindowWidth; x++)
				{
					var b = y * gameData.WindowWidth + x;
					buf[b].Attributes = AttributeAsShort(screen.Values[x, y].ForegroundColor, screen.Values[x, y].BackgroundColor);
					buf[b].Char.UnicodeChar = screen.Values[x, y].Value;
				}
			}

			NativeMethods.WriteConsoleOutput(h, buf,
				new Coord { X = (short)gameData.WindowWidth, Y = (short)gameData.WindowHeight },
				new Coord { X = 0, Y = 0 },
				ref rect);
		}

		private static short AttributeAsShort(ConsoleColor foreground, ConsoleColor background)
		{
			return (short)((short)foreground + 16 * (short)background);
		}
	}
}
