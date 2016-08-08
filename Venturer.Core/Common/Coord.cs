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

		public override string ToString()
		{
			return string.Format("{{{0}, {1}}}", X, Y);
		}
	}
}
