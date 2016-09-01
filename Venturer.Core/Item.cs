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
		public readonly ConsoleColor BackgroundColor;
		public readonly ConsoleColor UnlitBackgroundColor;
		public readonly Coord Location;

		private readonly bool _nameStartsWithVowelSound;

		public Item(string name, bool nameStartsWithVowelSound, char representation, ConsoleColor color, ConsoleColor unlitColor, ConsoleColor backgroundColor, ConsoleColor unlitBackgroundColor, Coord location)
		{
			Name = name;
			_nameStartsWithVowelSound = nameStartsWithVowelSound;
			Representation = representation;
			Color = color;
			UnlitColor = unlitColor;
			BackgroundColor = backgroundColor;
			UnlitBackgroundColor = unlitBackgroundColor;
			Location = location;
		}

		public string AsListItem => string.Format("{0} {1}", _nameStartsWithVowelSound ? "An" : "A", Name);
	}

	public class CollectibleItem : Item
	{
		public CollectibleItem(string name, bool nameStartsWithVowelSound, char representation, ConsoleColor color, ConsoleColor unlitColor, Coord location)
			: base(name, nameStartsWithVowelSound, representation, color, unlitColor, ConsoleColor.Black, ConsoleColor.Black, location)
		{
		}
	}
}
