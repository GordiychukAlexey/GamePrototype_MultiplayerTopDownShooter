using System.Collections.Generic;
using Photon.Deterministic;
using Quantum;
using UnityEngine.Scripting;

namespace Game
{
	[Preserve]
	public unsafe class EnemyHealthSystem : SystemMainThreadFilter<EnemyHealthSystem.Filter>, ISignalOnSpawnEnemy
		, ISignalOnComponentAdded<EnemyHealth>, ISignalOnComponentRemoved<EnemyHealth>
	{
		public struct Filter
		{
			public EntityRef Entity;
			public Transform2D* Transform;
			public EnemyCharacter* EnemyCharacter;
			public EnemyHealth* EnemyHealth;
		}

		private static List<EntityRef> damageSources = new();

		public void OnAdded(Frame f, EntityRef entity, EnemyHealth* component)
		{
			component->TakenDamage = f.AllocateDictionary<EntityRef, FP>();
		}

		public void OnRemoved(Frame f, EntityRef entity, EnemyHealth* component)
		{
			f.FreeDictionary(component->TakenDamage);
			component->TakenDamage = default;
		}


		public void OnSpawnEnemy(Frame frame, EntityRef enemy)
		{
			EnemyHealth* enemyHealth = frame.Unsafe.GetPointer<EnemyHealth>(enemy);
			EnemyHealthConfig enemyHealthConfig = frame.FindAsset<EnemyHealthConfig>(enemyHealth->EnemyHealthConfig.Id);

			enemyHealth->CurrentHealth = enemyHealthConfig.MaxHealth;
		}

		public override void Update(Frame frame, ref Filter filter)
		{
			ApplyDamage(frame, ref filter);
			CheckDeath(frame, ref filter);
		}

		private static void ApplyDamage(Frame frame, ref Filter filter)
		{
			var enemyHealth = filter.EnemyHealth;

			var damageSum = FP._0;

			var takenDamage = frame.ResolveDictionary(filter.EnemyHealth->TakenDamage);
			foreach (var pair in takenDamage)
			{
				damageSum += pair.Value;
			}

			if (enemyHealth->CurrentHealth <= damageSum)
			{
				damageSources.Clear();

				foreach (var pair in takenDamage)
				{
					damageSources.Add(pair.Key);
				}

				var randomDamageSource = damageSources[frame.RNG->Next(0, damageSources.Count)];
				if (frame.Unsafe.TryGetPointer(randomDamageSource, out PlayerCharacterScore* score))
				{
					score->Kills += 1;
				}

				frame.Signals.OnEnemyDeath(filter.Entity, randomDamageSource);
			}

			enemyHealth->CurrentHealth -= damageSum;

			takenDamage.Clear();
		}

		private static void CheckDeath(Frame frame, ref Filter filter)
		{
			if (filter.EnemyHealth->CurrentHealth <= FP._0)
			{
				frame.Destroy(filter.Entity);
			}
		}
	}
}