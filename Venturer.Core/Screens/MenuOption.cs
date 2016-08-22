using System;

namespace Venturer.Core.Screens
{
	internal class MenuOption
	{
	    public readonly string Text;
		public readonly Action Function;
		public readonly bool Bubble;
		public readonly bool DestroyWhenSelecting;

		public MenuOption(string text, Action function, bool bubble, bool destroyWhenSelecting)
		{
		    Text = text;
			Function = function;
			Bubble = bubble;
		    DestroyWhenSelecting = destroyWhenSelecting;
		}
	}
}
