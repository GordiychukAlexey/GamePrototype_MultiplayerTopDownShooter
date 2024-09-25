using Photon.Deterministic;

namespace Quantum
{
	public class PlayerCharacterMovementConfig : AssetObject
	{
		public FP BasePlayerMovingSpeed = FP.FromRoundedFloat_UNSAFE(1.0f);
		public FP PlayerMovingSpeedUpgradeStep = FP.FromRoundedFloat_UNSAFE(0.5f);
		public FP PlayerMovingSpeedUpgradeChance = FP.FromRoundedFloat_UNSAFE(0.6f);
	}
}