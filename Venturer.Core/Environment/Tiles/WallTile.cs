using System;
using Venturer.Core.Output;

namespace Venturer.Core.Environment.Tiles
{
	public class WallTile : Tile
	{
		public WallTile()
			: base(CodePoint.MediumShade, ConsoleColor.White, ConsoleColor.Gray, ConsoleColor.Black, ConsoleColor.Black)
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
						: (bottom ? CodePoint.BoxNS : CodePoint.BoxN)))
				: (right
					? (left
						? (bottom ? CodePoint.BoxSEW : CodePoint.BoxEW)
						: (bottom ? CodePoint.BoxSE : CodePoint.BoxE))
					: (left
						? (bottom ? CodePoint.BoxSW : CodePoint.BoxW)
						: (bottom ? CodePoint.BoxS : CodePoint.LowercaseO)));
		}

		internal override bool CanTraverse => false;
	}
}
