using System;
using System.Collections.Generic;
using System.Linq;
using Venturer.Core.Common;
using Venturer.Core.Input;
using Venturer.Core.Output;

namespace Venturer.Core.Screens
{
	public class MultiTextScreen : ViewPort
	{
		private readonly List<string> _strings;
		private int _stringIndex;

		internal override InputContext InputContext => InputContext.TextBox;
		internal override bool ShouldQuit => false;

		public MultiTextScreen(string text)
			: base(0, 0)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ArgumentException("No strings given");
			}

			_strings = new List<string> { text };
			_stringIndex = 0;
		}

		public MultiTextScreen(List<string> strings)
			: base(0, 0)
		{
			if (strings == null || !strings.Any())
			{
				throw new ArgumentException("No strings given");
			}

			_strings = strings;
			_stringIndex = 0;
		}

		internal override Screen ToScreen(int width, int height, int xOffset = 0, int yOffset = 0)
		{
			var glyphs = new Glyph[width, height];
			var continueChar = _stringIndex < _strings.Count - 1 ? CodePoint.TriangleRight : CodePoint.Square;
			Utilities.TextBox(glyphs, new[] { _strings[_stringIndex] }, width, height, continueChar);
			return new Screen(glyphs);
		}

		internal override bool ShouldDestroy => _stringIndex >= _strings.Count;

		internal override bool HandleInput(Command command)
		{
			_stringIndex++;
			return false;
		}

		internal override ViewPort GetAndClearNewViewPort()
		{
			return null;
		}
	}
}
