using Venturer.Core.Environment;

namespace Venturer.Core
{
	public interface IGameData
	{
		int WindowWidth { get; }
		int WindowHeight { get; }
		string GameTitle { get; }
		string WindowTitle { get; }
		ILevelFactory LevelFactory { get; }

		void LoadGame(int slot);
		void SaveGame(int slot);
	}
}
