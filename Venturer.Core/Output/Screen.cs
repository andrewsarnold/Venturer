namespace Venturer.Core.Output
{
	public class Screen
	{
		public readonly int Width;
		public readonly int Height;
		public readonly int XOffset;
		public readonly int YOffset;

		public readonly Glyph[,] Values;

		public Screen(Glyph[,] values, int xOffset = 0, int yOffset = 0)
		{
			Width = values.GetLength(0);
			Height = values.GetLength(1);
			XOffset = xOffset;
			YOffset = yOffset;
			Values = values;
		}

		public static void AddChar(Glyph[,] chars, int x, int y, Glyph glyph)
		{
			if (x < 0 || x >= chars.GetLength(0) || y < 0 || y >= chars.GetLength(1))
			{
				return;
			}

			chars[x, y] = glyph;
		}
	}
}
