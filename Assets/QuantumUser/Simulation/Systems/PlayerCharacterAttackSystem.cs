using Quantum;
using UnityEngine.Scripting;

namespace Game
{
	[Preserve]
	public unsafe class PlayerCharacterAttackSystem : SystemMainThreadFilter<PlayerCharacterAttackSystem.Filter>,
		ISignalOnSpawnPlayerCharacter
	{
		public struct Filter
		{
			public EntityRef Entity;
			public Transform2D* Transform;
			public PlayerCharacter* PlayerCharacter;
			public PlayerCharacterAttack* PlayerCharacterAttack;
			public PlayerCharacterTargets* PlayerCharacterTargets;
		}

		public void OnSpawnPlayerCharacter(Frame frame, EntityRef entityRef)
		{
			PlayerCharacterAttack* attack = frame.Unsafe.GetPointer<PlayerCharacterAttack>(entityRef);
			PlayerCharacterAttackConfig config = frame.FindAsset<PlayerCharacterAttackConfig>(attack->AttackConfig.Id);

			attack->AttackRadius = config.BaseAttackRadius;
			attack->DamagePerSecond = config.BaseDamagePerSecond;
		}

		public override void Update(Frame f, ref Filter filter)
		{
			var closestEnemies = f.ResolveDictionary(filter.PlayerCharacterTargets->ClosestEnemies);

			var attackRadiusSquared = filter.PlayerCharacterAttack->AttackRadius * filter.PlayerCharacterAttack->AttackRadius;

			foreach (var closestEnemy in closestEnemies)
			{
				if (closestEnemy.Value < attackRadiusSquared)
				{
					f.Unsafe.GetPointer<EnemyHealth>(closestEnemy.Key)->AddDamage(f, filter.PlayerCharacterAttack->DamagePerSecond * f.DeltaTime, filter.Entity);
				}
			}
		}
	}
}