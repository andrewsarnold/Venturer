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

        public Item(string name, char representation, ConsoleColor color, ConsoleColor unlitColor, Coord location)
        {
            Name = name;
            Representation = representation;
            Color = color;
            UnlitColor = unlitColor;
            Location = location;
        }
    }
}
