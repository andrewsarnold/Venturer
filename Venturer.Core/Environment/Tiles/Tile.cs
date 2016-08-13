using System;
using Venturer.Core.Output;

namespace Venturer.Core.Environment.Tiles
{
	public abstract class Tile
	{
		private readonly ConsoleColor _darkBackgroundColor;
		private readonly ConsoleColor _darkForegroundColor;
		private readonly ConsoleColor _lightBackgroundColor;
		private readonly ConsoleColor _lightForegroundColor;
		private bool _hasSeen;

		protected Tile(char representation,
			ConsoleColor lightForegroundColor, ConsoleColor darkForegroundColor,
			ConsoleColor lightBackgroundColor, ConsoleColor darkBackgroundColor)
		{
			_hasSeen = false;
			Representation = representation;
			_lightForegroundColor = lightForegroundColor;
			_darkForegroundColor = darkForegroundColor;
			_lightBackgroundColor = lightBackgroundColor;
			_darkBackgroundColor = darkBackgroundColor;
		}

		protected Tile(char representation,
			ConsoleColor lightForegroundColor, ConsoleColor darkForegroundColor)
		{
			_hasSeen = false;
			Representation = representation;
			_lightForegroundColor = lightForegroundColor;
			_darkForegroundColor = darkForegroundColor;
			_lightBackgroundColor = lightForegroundColor;
			_darkBackgroundColor = darkForegroundColor;
		}

		protected char Representation { private get; set; }

		internal void SetAsSeen()
		{
			_hasSeen = true;
		}

		internal Glyph ToCharacter(bool isVisible)
		{
			return new Glyph(Representation,
				isVisible
					? _lightForegroundColor
					: _hasSeen
						? _darkForegroundColor
						: ConsoleColor.Black,
				isVisible
					? _lightBackgroundColor
					: _hasSeen
						? _darkBackgroundColor
						: ConsoleColor.Black);
		}

		internal abstract bool CanTraverse { get; }
	}
}
