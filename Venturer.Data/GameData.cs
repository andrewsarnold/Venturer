using Venturer.Core;
using Venturer.Core.Environment;

namespace Venturer.Data
{
    public class GameData : IGameData
    {
	    public int WindowWidth => 76;
	    public int WindowHeight => 24;
	    public string GameTitle => "Test Game";
		public ILevelFactory LevelFactory { get; }
		
	    public GameData()
	    {
			LevelFactory = new LevelFactory();
	    }
	}
}
