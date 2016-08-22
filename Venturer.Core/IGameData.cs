using Venturer.Core.Environment;

namespace Venturer.Core
{
	public interface IGameData
	{
		string GameTitle { get; }
		ILevelFactory LevelFactory { get; }

		void LoadGame(int slot);
	}
}
