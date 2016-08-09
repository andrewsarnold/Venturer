using System;
using Venturer.Core.Output;

namespace Venturer.Core.Environment.Tiles
{
	public class FloorTile : Tile
	{
		public FloorTile()
			: base(CodePoint.FullBlock, ConsoleColor.Gray, ConsoleColor.DarkGray)
		{
		}

		internal override bool CanTraverse
		{
			get { return true; }
		}
	}
}
