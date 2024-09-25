using System.Collections.Generic;
using Photon.Deterministic;

namespace Quantum
{
	public class GameConfig : AssetObject
	{
		public List<AssetRef<EntityPrototype>> EnemyPrototypes;
		public FP EnemySpawnDistanceToCenter = FP._9;
		public int EnemiesInitialCount = 5;
	}
}