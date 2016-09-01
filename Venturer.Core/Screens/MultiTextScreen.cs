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
		private Action _onFinished;
		private List<string> _strings;
		private int _stringIndex;

		internal override InputContext InputContext => InputContext.TextBox;
		internal override bool ShouldQuit => false;

		public MultiTextScreen(string text)
			: base(0, 0)
		{
			Constructor(new List<string> { text }, () => { });
		}

		public MultiTextScreen(string text, Action onFinished)
			: base(0, 0)
		{
			Constructor(new List<string> { text }, onFinished);
		}

		public MultiTextScreen(List<string> strings)
			: base(0, 0)
		{
			Constructor(strings, () => { });
		}

		private void Constructor(List<string> strings, Action onFinished)
		{
			_onFinished = onFinished;
			if (strings == null || !strings.Any() || strings.Any(string.IsNullOrWhiteSpace))
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
			_onFinished();
			return true;
		}

		internal override ViewPort GetAndClearNewViewPort()
		{
			return null;
		}
	}
}
