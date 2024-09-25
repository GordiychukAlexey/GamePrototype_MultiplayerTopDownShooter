using Photon.Deterministic;
using Quantum;
using UnityEngine.Scripting;

namespace Game
{
	[Preserve]
	public unsafe class PlayerCharacterSpawnSystem : SystemSignalsOnly, ISignalOnSpawnPlayerCharacter
	{
		public void OnSpawnPlayerCharacter(Frame f, EntityRef entry)
		{
			Transform2D* transform = f.Unsafe.GetPointer<Transform2D>(entry);
			transform->Position = FPVector2.Zero;
			transform->Teleport(f, transform);

			f.Unsafe.GetPointer<PhysicsBody2D>(entry)->Velocity = default;
			f.Unsafe.GetPointer<PhysicsBody2D>(entry)->AngularVelocity = default;
			f.Unsafe.GetPointer<PhysicsCollider2D>(entry)->Enabled = true;
		}
	}
}