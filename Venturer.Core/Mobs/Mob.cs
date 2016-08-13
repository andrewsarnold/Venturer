using System;
using System.Collections.Generic;
using Venturer.Core.Common;
using Venturer.Core.Environment;
using Venturer.Core.Output;

namespace Venturer.Core.Mobs
{
	public abstract class Mob
	{
		/// <summary>
		/// Gets or sets the level-based coordinates of this mob.
		/// </summary>
		public Coord Position;

		protected Mob(Coord pos)
		{
			Position = pos;
		}

		public abstract void Draw(Glyph[,] chars, int x, int y, ConsoleColor backgroundColor);

		/// <summary>
		/// Fired when the player runs into the NPC.
		/// </summary>
		/// <param name="direction">The direction the player is moving.</param>
		/// <returns>Any messages to display.</returns>
		public abstract IEnumerable<string> Interact(Direction direction);
	}
}
