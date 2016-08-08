namespace Venturer.Core.Output
{
	public static class CodePoint
	{
		public static char Smiley { get { return (char)0x01; } }

		public static char TriangleRight { get { return (char)0x10; } }
		public static char TriangleLeft { get { return (char)0x11; } }
		public static char TriangleUp { get { return (char)0x1e; } }
		public static char TriangleDown { get { return (char)0x1f; } }

		public static char LightShade { get { return (char)0xb0; } }
		public static char MediumShade { get { return (char)0xb1; } }
		public static char DarkShade { get { return (char)0xb2; } }

		public static char BoxNS { get { return (char)0xb3; } }
		public static char BoxNSW { get { return (char)0xb4; } }
		public static char BoxSW { get { return (char)0xbf; } }
		public static char BoxNE { get { return (char)0xc0; } }
		public static char BoxNEW { get { return (char)0xc1; } }
		public static char BoxSEW { get { return (char)0xc2; } }
		public static char BoxNSE { get { return (char)0xc3; } }
		public static char BoxEW { get { return (char)0xc4; } }
		public static char BoxNSEW { get { return (char)0xc5; } }
		public static char BoxNW { get { return (char)0xd9; } }
		public static char BoxSE { get { return (char)0xda; } }

		public static char FullBlock { get { return (char)0xdb; } }

		public static char Intersection { get { return (char)0xef; } }

		public static char Square { get { return (char)0xfe; } }
		
		public static char LowercaseO { get { return 'o'; } }
	}
}
