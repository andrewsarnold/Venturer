using System.Collections.Generic;
using Venturer.Core;
using Venturer.Core.Environment;

namespace Venturer.Data
{
	public class GameData : IGameData
	{
		public int WindowWidth => 76;
		public int WindowHeight => 24;
		public string GameTitle => "Test Game";
		public string WindowTitle { get; private set; }
		public ILevelFactory LevelFactory { get; }
		public List<CollectibleItem> Inventory { get; }
		
		public GameData()
		{
			WindowTitle = "New game";
			LevelFactory = new LevelFactory();
			Inventory = new List<CollectibleItem>();
		}

		public void LoadGame(int slot)
		{
			WindowTitle = $"Loaded from slot {slot}";
		}

		public void SaveGame(int slot)
		{
		}
	}
}
