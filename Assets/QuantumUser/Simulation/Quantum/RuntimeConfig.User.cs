using Game;

namespace Quantum
{
	public partial class RuntimeConfig
	{
		/// <summary>
		/// Reference to the game configuration asset, specifying various settings for the Asteroids game.
		/// </summary>
		public AssetRef<GameConfig> GameConfig;

		/// <summary>
		/// Reference to the default player avatar prototype to be used when creating player entities.
		/// </summary>
		public AssetRef<EntityPrototype> PlayerCharacterPrefab;
	}
}