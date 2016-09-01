using System.Linq;
using Venturer.Core.Environment;
using Venturer.Data.Rooms;

namespace Venturer.Data
{
	internal class LevelFactory : ILevelFactory
	{
		public Level GetLevel()
		{
			var rooms = new IRoomFactory[] { new Room1(), new Room2() }.Select(r => r.MakeRoom()).ToDictionary(r => r.Name);
			return new Level(rooms, "start");
		}
	}
}
