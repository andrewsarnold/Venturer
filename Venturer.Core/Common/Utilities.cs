using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Venturer.Core.Output;

namespace Venturer.Core.Common
{
	internal static class Utilities
	{
		internal static void TextBox(Glyph[,] chars, IEnumerable<string> text, int width, int height, char continueChar = ' ')
		{
			var maxWidth = width / 2;
			var lines = text.Select(t => SplitString(t, maxWidth - 4)).SelectMany(t => t).ToList();
			maxWidth = lines.Max(l => l.Length) + (continueChar != ' ' ? 2 : 0);

			var topY = height / 2 - lines.Count / 2;
			var leftX = width / 2 - maxWidth / 2;

			// Draw interior
			for (var l = 0; l < lines.Count; l++)
			{
				WriteTextLine(chars, new Coord(leftX, topY + l), lines[l], maxWidth);
			}

			// Draw corners
			Screen.AddChar(chars, leftX - 1, topY - 1, new Glyph(CodePoint.BoxSE));
			Screen.AddChar(chars, leftX + maxWidth, topY - 1, new Glyph(CodePoint.BoxSW));
			Screen.AddChar(chars, leftX - 1, topY + lines.Count, new Glyph(CodePoint.BoxNE));
			Screen.AddChar(chars, leftX + maxWidth, topY + lines.Count, new Glyph(CodePoint.BoxNW));

			// Draw top and bottom
			for (var x = 0; x < maxWidth; x++)
			{
				Screen.AddChar(chars, x + leftX, topY - 1, new Glyph(CodePoint.BoxEW));
				Screen.AddChar(chars, x + leftX, topY + lines.Count, new Glyph(CodePoint.BoxEW));
			}

			// Draw left and right
			for (var y = 0; y < lines.Count; y++)
			{
				Screen.AddChar(chars, leftX - 1, y + topY, new Glyph(CodePoint.BoxNS));
				Screen.AddChar(chars, leftX + maxWidth, y + topY, new Glyph(CodePoint.BoxNS));
			}

			// Draw continue character
			if (continueChar != ' ')
			{
				Screen.AddChar(chars, leftX + maxWidth - 1, lines.Count + topY - 1, new Glyph(continueChar));
			}
		}

		internal static void WriteTextLine(Glyph[,] chars, Coord startLocation, string text, int padToWidth = 0)
		{
			for (var x = 0; x < (padToWidth > text.Length ? padToWidth : text.Length); x++)
			{
				chars[startLocation.X + x, startLocation.Y] = new Glyph(x >= text.Length ? ' ' : text[x]);
			}
		}

		internal static string Stylize(string text)
		{
			var chars = text.ToUpper(CultureInfo.CurrentCulture).ToCharArray().ToList();
			for (var i = 1; i < chars.Count; i += 2)
			{
				chars.Insert(i, ' ');
			}
			return string.Join("", chars);
		}

		private static List<string> SplitString(string text, int maxWidth)
		{
			var words = text.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			var result = new List<string>();
			var currentWord = "";

			foreach (var w in words)
			{
				if (currentWord.Length + w.Length + 1 < maxWidth)
				{
					currentWord += (string.IsNullOrEmpty(currentWord) ? "" : " ") + w;
				}
				else
				{
					result.Add(currentWord);
					currentWord = w;
				}
			}

			if (!string.IsNullOrEmpty(currentWord))
			{
				result.Add(currentWord);
			}

			return result;
		}
	}
}
