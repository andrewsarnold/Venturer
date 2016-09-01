using System;
using Venturer.Core.Common;

namespace Venturer.Core
{
	public class Item
	{
		public readonly string Name;
		public readonly char Representation;
		public readonly ConsoleColor Color;
		public readonly ConsoleColor UnlitColor;
		public readonly Coord Location;

		private readonly bool _nameStartsWithVowelSound;

		public Item(string name, bool nameStartsWithVowelSound, char representation, ConsoleColor color, ConsoleColor unlitColor, Coord location)
		{
			Name = name;
			_nameStartsWithVowelSound = nameStartsWithVowelSound;
			Representation = representation;
			Color = color;
			UnlitColor = unlitColor;
			Location = location;
		}

		public string AsListItem => string.Format("{0} {1}", _nameStartsWithVowelSound ? "An" : "A", Name);
	}
}
