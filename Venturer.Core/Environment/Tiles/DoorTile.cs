using System;
using Venturer.Core.Output;

namespace Venturer.Core.Environment.Tiles
{
	public class DoorTile : Tile
	{
		public DoorTile()
			: base(CodePoint.CapitalPi, ConsoleColor.Blue, ConsoleColor.DarkBlue, ConsoleColor.DarkGray, ConsoleColor.Black)
		{
		}

		internal override bool CanTraverse
		{
			get { return true; }
		}
	}
}
