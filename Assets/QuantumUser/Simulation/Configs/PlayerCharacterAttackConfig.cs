using Photon.Deterministic;

namespace Quantum
{
	public class PlayerCharacterAttackConfig : AssetObject
	{
		public FP BaseAttackRadius = FP.FromRoundedFloat_UNSAFE(2.0f);
		public FP AttackRadiusUpgradeStep = FP.FromRoundedFloat_UNSAFE(1.0f);
		public FP AttackRadiusUpgradeChance = FP.FromRoundedFloat_UNSAFE(0.1f);
		public FP BaseDamagePerSecond = FP.FromRoundedFloat_UNSAFE(10.0f);
		public FP DamagePerSecondUpgradeStep = FP.FromRoundedFloat_UNSAFE(10.0f);
		public FP DamagePerSecondUpgradeChance = FP.FromRoundedFloat_UNSAFE(0.3f);
	}
}