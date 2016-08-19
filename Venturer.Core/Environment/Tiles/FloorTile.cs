using System;
using Venturer.Core.Output;

namespace Venturer.Core.Environment.Tiles
{
	public class FloorTile : Tile
	{
		public FloorTile()
			: base(CodePoint.FullBlock, ConsoleColor.DarkGray, ConsoleColor.Black)
		{
		}

		internal override bool CanTraverse
		{
			get { return true; }
		}
	}
}
