using System.Collections.Generic;
using Photon.Deterministic;
using Quantum;
using UnityEngine.Scripting;

namespace Game
{
	[Preserve]
	public unsafe class PlayerCharacterTargetSelectorSystem : SystemMainThreadFilter<PlayerCharacterTargetSelectorSystem.Filter>,
		ISignalOnComponentAdded<PlayerCharacterTargets>, ISignalOnComponentRemoved<PlayerCharacterTargets>
	{
		public struct Filter
		{
			public EntityRef Entity;
			public Transform2D* Transform;
			public PlayerCharacter* PlayerCharacter;
			public PlayerCharacterTargets* PlayerCharacterTargets;
		}

		private List<(EntityRef, FP)> distances = new();

		public void OnAdded(Frame f, EntityRef entity, PlayerCharacterTargets* component)
		{
			var playerCharacterTargets = f.Unsafe.GetPointer<PlayerCharacterTargets>(entity);
			var playerCharacterTargetsConfig = f.FindAsset<PlayerCharacterTargetsConfig>(playerCharacterTargets->TargetsConfig.Id);
			component->ClosestEnemies = f.AllocateDictionary<EntityRef, FP>(playerCharacterTargetsConfig.MaximumTargetsCount);
		}

		public void OnRemoved(Frame f, EntityRef entity, PlayerCharacterTargets* component)
		{
			f.FreeDictionary(component->ClosestEnemies);
			component->ClosestEnemies = default;
		}

		public override void Update(Frame f, ref Filter filter)
		{
			var filtered = f.Filter<EnemyCharacter, Transform2D>();

			var closestEnemies = f.ResolveDictionary(filter.PlayerCharacterTargets->ClosestEnemies);

			distances.Clear();
			closestEnemies.Clear();

			var playerCharacterTargetsConfig = f.FindAsset<PlayerCharacterTargetsConfig>(filter.PlayerCharacterTargets->TargetsConfig.Id);

			var freeTargetSlots = playerCharacterTargetsConfig.MaximumTargetsCount;

			while (filtered.NextUnsafe(out var e, out var enemyCharacter, out var transform))
			{
				var distance = FPVector2.DistanceSquared(filter.Transform->Position, transform->Position);

				if (freeTargetSlots > 0)
				{
					distances.Add((e, distance));
					freeTargetSlots--;
				}
				else
				{
					for (int i = 0; i < playerCharacterTargetsConfig.MaximumTargetsCount; i++)
					{
						if (distances[i].Item2 > distance)
						{
							distances[i] = (e, distance);
							break;
						}
					}
				}
			}

			for (int i = 0; i < distances.Count; i++)
			{
				closestEnemies.Add(distances[i].Item1, distances[i].Item2);
			}
		}
	}
}