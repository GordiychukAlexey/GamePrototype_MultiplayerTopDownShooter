using Photon.Deterministic;

namespace Quantum
{
	public unsafe partial struct EnemyHealth
	{
		public void AddDamage(Frame f, FP damageValue, EntityRef damageSource)
		{
			var takenDamage = f.ResolveDictionary(TakenDamage);

			if (!takenDamage.ContainsKey(damageSource))
			{
				takenDamage.Add(damageSource, damageValue);
			}
			else
			{
				takenDamage[damageSource] += damageValue;
			}
		}
	}
}