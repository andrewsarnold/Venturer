using System;
using Venturer.Core.Output;

namespace Venturer.Core.Environment.Tiles
{
	public class FloorTile : Tile
	{
		public FloorTile()
			: base(CodePoint.FullBlock, ConsoleColor.DarkBlue, ConsoleColor.Black)
		{
		}

		internal override bool CanTraverse => true;
	}
}
