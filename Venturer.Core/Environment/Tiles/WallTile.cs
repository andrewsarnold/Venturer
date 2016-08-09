using System;
using Venturer.Core.Output;

namespace Venturer.Core.Environment.Tiles
{
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

		internal override bool CanTraverse
		{
			get { return true; }
		}
	}
}
