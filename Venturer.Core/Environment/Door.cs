using Venturer.Core.Common;

namespace Venturer.Core.Environment
{
	internal class Door
	{
		internal Coord Location;
		internal string TargetRoom;
		internal Coord TargetCoord;

		public Door(Coord location, string targetRoom, Coord targetCoord)
		{
			Location = location;
			TargetRoom = targetRoom;
			TargetCoord = targetCoord;
		}
	}
}
