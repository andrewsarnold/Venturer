using Venturer.Core;
using Venturer.Core.Environment;

namespace Venturer.Data
{
	public class GameData : IGameData
	{
		public string GameTitle { get; private set; }
		public ILevelFactory LevelFactory { get; }

		public GameData()
		{
			GameTitle = "New game";
			LevelFactory = new LevelFactory();
		}

		public void LoadGame(int slot)
		{
			GameTitle = $"Loaded from slot {slot}";
		}
	}
}
