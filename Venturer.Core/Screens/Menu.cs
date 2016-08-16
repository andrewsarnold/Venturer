using System;
using System.Collections.Generic;
using System.Linq;
using Venturer.Core.Common;
using Venturer.Core.Input;
using Venturer.Core.Output;

namespace Venturer.Core.Screens
{
	internal class Menu : ViewPort
	{
		private readonly List<MenuOption> _options;
		private readonly Action _onEscape;
		private ViewPort _newViewPort;
		private bool _handled;

		public Menu(int width, int height, List<MenuOption> options, Action onEscape)
			: base(width, height)
		{
			if (options.Count > 9)
			{
				throw new ArgumentException("Too many options");
			}

			_options = options;
			_onEscape = onEscape;
		}

		internal override bool HandleInput(Command command)
		{
			if (command == Command.Quit)
			{
				_onEscape();
				_handled = true;
				return false;
			}

			var optionCount = _options.Count;
			const Command commandMin = Command.MenuItem1;
			var commandMax = commandMin + optionCount - 1;
			if (command >= commandMin && command <= commandMax)
			{
				var selection = command - commandMin;
				_options[selection].Function();
				_handled = true;
				return _options[selection].Bubble;
			}

			return false;
		}

		internal override Screen ToScreen(int width, int height, int xOffset = 0, int yOffset = 0)
		{
			var glyphs = new Glyph[Width, Height];
			Utilities.TextBox(glyphs, _options.Select((t, i) => string.Format("{0} {1}", i + 1, t.Text)).ToList(), Width, Height);
			return new Screen(glyphs);
		}

		internal override ViewPort GetAndClearNewViewPort()
		{
			var newViewPort = _newViewPort;
			_newViewPort = null;
			return newViewPort;
		}

		internal override bool ShouldDestroy
		{
			get { return _handled; }
		}

		internal override InputContext InputContext
		{
			get { return InputContext.Menu; }
		}
	}
}
