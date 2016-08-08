using System;
using Venturer.Core.Output;

namespace Venturer.Core.Screens
{
	internal class TestScreen : ViewPort
	{
		internal override bool HandleInput(ConsoleKeyInfo key)
		{
			return false;
		}

		internal override Screen ToScreen(int width, int height, int xOffset = 0, int yOffset = 0)
		{
			var chars = new Glyph[width, height];
			for (var x = 0; x < width; x++)
			{
				for (var y = 0; y < height; y++)
				{
					chars[x,y] = new Glyph(' ');
				}
			}
			chars[4,4] = new Glyph('h');
			chars[5,4] = new Glyph('e');
			chars[6,4] = new Glyph('l');
			chars[7,4] = new Glyph('l');
			chars[8,4] = new Glyph('o');
			return new Screen(chars, xOffset, yOffset);
		}

		internal override ViewPort NewViewPort
		{
			get { return null; }
		}

		internal override bool ShouldDestroy
		{
			get { return false; }
		}
	}
}
