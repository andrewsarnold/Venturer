using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

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
        public static void Main(string[] args)
        {
            SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            if (h.IsInvalid)
            {
                Console.WriteLine("Error initializing.");
                Console.ReadKey();
                return;
            }

            Console.OutputEncoding = Encoding.UTF8;
            for (var i = 20; i <= 10000; i++)
            {
                Console.WriteLine("{0}\t\\u{1}\t{2}",
                    i,
                    i.ToString("x4"),
                    char.ConvertFromUtf32(i));

                if (i % 20 == 0)
                {
                    Console.ReadKey();
                }
            }
            Console.ReadKey();
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
