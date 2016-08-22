using System;

namespace Venturer.Core.Output
{
	public struct Glyph
	{
		public readonly ConsoleColor BackgroundColor;
		public readonly ConsoleColor ForegroundColor;
		public readonly char Value;

		public Glyph(char value)
		{
			Value = value;
			ForegroundColor = ConsoleColor.Gray;
			BackgroundColor = ConsoleColor.Black;
		}

		public Glyph(char value, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
		{
			Value = value;
			ForegroundColor = foregroundColor;
			BackgroundColor = backgroundColor;
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
