using System;
using Venturer.Core.Output;

namespace Venturer.Core.Environment.Tiles
{
	public class FloorTile : Tile
	{
		public FloorTile()
			: base(CodePoint.Space, ConsoleColor.DarkGray, ConsoleColor.Black, ConsoleColor.Black, ConsoleColor.Black)
		{
		}

		internal void SetRepresentation(char c)
		{
			Representation = c;
		}

		internal override bool CanTraverse => true;
	}
}
