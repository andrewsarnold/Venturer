using System;
using Venturer.Core.Output;

namespace Venturer.Core.Environment
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

		public void SetAsSeen()
		{
			_hasSeen = true;
		}

		public Glyph ToCharacter(bool isVisible)
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
	}

	public class FloorTile : Tile
	{
		public FloorTile()
			: base(CodePoint.FullBlock, ConsoleColor.Gray, ConsoleColor.DarkGray)
		{
		}
	}

	public class WallTile : Tile
	{
		public WallTile()
			: base(CodePoint.MediumShade, ConsoleColor.White, ConsoleColor.Gray, ConsoleColor.Gray, ConsoleColor.DarkGray)
		{
		}

		public void SetNeighbors(bool top, bool right, bool bottom, bool left)
		{
			Representation = top
				? (right
					? (left
						? (bottom ? CodePoint.BoxNSEW : CodePoint.BoxNEW)
						: (bottom ? CodePoint.BoxNSE : CodePoint.BoxNE))
					: (left
						? (bottom ? CodePoint.BoxNSW : CodePoint.BoxNW)
						: CodePoint.BoxNS))
				: (right
					? (left
						? (bottom ? CodePoint.BoxSEW : CodePoint.BoxEW)
						: (bottom ? CodePoint.BoxSE : CodePoint.BoxEW))
					: (left
						? (bottom ? CodePoint.BoxSW : CodePoint.BoxEW)
						: (bottom ? CodePoint.BoxNS : CodePoint.LowercaseO)));
		}
	}
}
