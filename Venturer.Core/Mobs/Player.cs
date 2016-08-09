using System;
using System.Collections.Generic;
using Venturer.Core.Common;
using Venturer.Core.Environment;
using Venturer.Core.Output;

namespace Venturer.Core.Mobs
{
	public class Player : Mob
	{
		public Player(Coord pos)
			: base(pos)
		{
		}

		public override void Draw(Glyph[,] chars, int x, int y, ConsoleColor backgroundColor)
		{
			Screen.AddChar(chars, x, y, new Glyph(CodePoint.Smiley, ConsoleColor.White, backgroundColor));
		}

		public override IEnumerable<string> Interact(Direction direction)
		{
			throw new NotImplementedException();
		}
	}
}
