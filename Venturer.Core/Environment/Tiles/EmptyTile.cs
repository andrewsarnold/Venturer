using System;
using Venturer.Core.Output;

namespace Venturer.Core.Environment.Tiles
{
	public class EmptyTile : Tile
	{
		public EmptyTile()
			: base(CodePoint.Space, ConsoleColor.Black, ConsoleColor.Black, ConsoleColor.Black, ConsoleColor.Black)
		{
		}

		internal override bool CanTraverse => false;
	}
}
