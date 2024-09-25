using Quantum;
using UnityEngine.Scripting;

namespace Game
{
	[Preserve]
	public unsafe class PlayerCharacterUpgradeSystem : SystemMainThreadFilter<PlayerCharacterUpgradeSystem.Filter>
	{
		public struct Filter
		{
			public EntityRef Entity;
			public Transform2D* Transform;
			public PlayerLink* PlayerLink;
			public PlayerCharacterMovement* PlayerCharacterMovement;
			public PlayerCharacterAttack* PlayerCharacterAttack;
		}

		public override void Update(Frame f, ref Filter filter)
		{
			Input* input = f.GetPlayerInput(filter.PlayerLink->PlayerRef);

			if (input->Upgrade.WasPressed)
			{
				var movementConfig = f.FindAsset<PlayerCharacterMovementConfig>(filter.PlayerCharacterMovement->MovementConfig.Id);
				var attackConfig = f.FindAsset<PlayerCharacterAttackConfig>(filter.PlayerCharacterAttack->AttackConfig.Id);

				if (f.RNG->Next() < movementConfig.PlayerMovingSpeedUpgradeChance)
				{
					filter.PlayerCharacterMovement->MovingSpeed += movementConfig.PlayerMovingSpeedUpgradeStep;
				}

				if (f.RNG->Next() < attackConfig.AttackRadiusUpgradeChance)
				{
					filter.PlayerCharacterAttack->AttackRadius += attackConfig.AttackRadiusUpgradeStep;
				}

				if (f.RNG->Next() < attackConfig.DamagePerSecondUpgradeChance)
				{
					filter.PlayerCharacterAttack->DamagePerSecond += attackConfig.DamagePerSecondUpgradeStep;
				}
			}
		}
	}
}