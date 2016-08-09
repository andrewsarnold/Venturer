using System;

namespace Venturer.Core.Common
{
	public struct Coord
	{
		public Coord(int x, int y)
			: this()
		{
			X = x;
			Y = y;
		}

		public int X { get; private set; }
		public int Y { get; private set; }

		internal static double Distance(Coord c1, Coord c2)
		{
			return Math.Sqrt(Math.Pow(c2.X - c1.X, 2) + Math.Pow(c2.Y - c1.Y, 2));
		}

		public override string ToString()
		{
			return string.Format("{{{0}, {1}}}", X, Y);
		}
	}
}
