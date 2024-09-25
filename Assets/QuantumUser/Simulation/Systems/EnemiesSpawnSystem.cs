using Photon.Deterministic;
using Quantum;
using UnityEngine.Scripting;

namespace Game
{
	[Preserve]
	public unsafe class EnemiesSpawnSystem : SystemSignalsOnly, ISignalOnComponentRemoved<EnemyCharacter>
	{
		public override void OnInit(Frame f)
		{
			SpawnInitialEnemies(f);
		}

		private void SpawnInitialEnemies(Frame f)
		{
			GameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);

			for (int i = 0; i < config.EnemiesInitialCount; i++)
			{
				SpawnRandomEnemy(f);
			}
		}

		public void OnRemoved(Frame f, EntityRef entity, EnemyCharacter* component)
		{
			SpawnRandomEnemy(f);
		}

		private void SpawnRandomEnemy(Frame f)
		{
			GameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);
			var enemyPrototypes = config.EnemyPrototypes;
			EntityRef enemy = f.Create(enemyPrototypes[f.Global->RngSession.Next(0, enemyPrototypes.Count)]);
			Transform2D* enemyTransform = f.Unsafe.GetPointer<Transform2D>(enemy);

			enemyTransform->Position = GetRandomEdgePointOnCircle(f, config.EnemySpawnDistanceToCenter);
			enemyTransform->Rotation = FP._0;

			f.Signals.OnSpawnEnemy(enemy);
		}

		private static FPVector2 GetRandomEdgePointOnCircle(Frame f, FP radius)
		{
			return FPVector2.Rotate(FPVector2.Up * radius, f.RNG->Next() * FP.PiTimes2);
		}
	}
}