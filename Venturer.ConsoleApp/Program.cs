using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using Venturer.Core;
using Venturer.Core.Output;

namespace Venturer.ConsoleApp
{
	public static class Program
	{
		#region DllImports

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern SafeFileHandle CreateFile(
			string fileName,
			[MarshalAs(UnmanagedType.U4)] uint fileAccess,
			[MarshalAs(UnmanagedType.U4)] uint fileShare,
			IntPtr securityAttributes,
			[MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
			[MarshalAs(UnmanagedType.U4)] int flags,
			IntPtr template);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool WriteConsoleOutput(
			SafeFileHandle hConsoleOutput,
			CharInfo[] lpBuffer,
			Coord dwBufferSize,
			Coord dwBufferCoord,
			ref SmallRect lpWriteRegion);

		#endregion

		[STAThread]
		public static void Main()
		{
			var h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
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

				game.Draw += () => { Write(game.ToScreen(), h); };

				bool shouldQuit;
				do
				{
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

			WriteConsoleOutput(h, buf,
				new Coord { X = Game.WindowWidth, Y = Game.WindowHeight },
				new Coord { X = 0, Y = 0 },
				ref rect);
		}

		private static short AttributeAsShort(ConsoleColor foreground, ConsoleColor background)
		{
			return (short)((short)foreground + 16 * (short)background);
		}
	}

	#region Structs

	[StructLayout(LayoutKind.Sequential)]
	public struct Coord
	{
		public short X;
		public short Y;
	};

	[StructLayout(LayoutKind.Explicit)]
	public struct CharUnion
	{
		[FieldOffset(0)]
		public byte AsciiChar;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct CharInfo
	{
		[FieldOffset(0)]
		public CharUnion Char;
		[FieldOffset(2)]
		public short Attributes;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SmallRect
	{
		public short Left;
		public short Top;
		public short Right;
		public short Bottom;
	}

	#endregion
}
