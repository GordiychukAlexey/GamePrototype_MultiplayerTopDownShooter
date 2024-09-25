using Game;

namespace Quantum
{
	using System.Collections.Generic;

	public static partial class DeterministicSystemSetup
	{
		static partial void AddSystemsUser(ICollection<SystemBase> systems, RuntimeConfig gameConfig, SimulationConfig simulationConfig, SystemsConfig systemsConfig)
		{
			systems.Add(new PlayerSystem());
			systems.Add(new PlayerCharacterSpawnSystem());
			systems.Add(new PlayerCharacterMovementSystem());
			systems.Add(new PlayerCharacterTargetSelectorSystem());
			systems.Add(new PlayerCharacterAttackSystem());
			systems.Add(new PlayerCharacterUpgradeSystem());
			systems.Add(new EnemiesSpawnSystem());
			systems.Add(new EnemyHealthSystem());
		}
	}
}

