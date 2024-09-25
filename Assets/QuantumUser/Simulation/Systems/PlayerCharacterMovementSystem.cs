using Photon.Deterministic;
using Quantum;
using UnityEngine.Scripting;
using Input = Quantum.Input;

namespace Game
{
	[Preserve]
	public unsafe class PlayerCharacterMovementSystem : SystemMainThreadFilter<PlayerCharacterMovementSystem.Filter>,
		ISignalOnSpawnPlayerCharacter
	{
		public struct Filter
		{
			public EntityRef Entity;
			public Transform2D* Transform;
			public PlayerLink* PlayerLink;
			public PlayerCharacter* PlayerCharacter;
			public PlayerCharacterMovement* PlayerCharacterMovement;
			public PhysicsBody2D* PhysicsBody;
		}

		public void OnSpawnPlayerCharacter(Frame frame, EntityRef entityRef)
		{
			PlayerCharacterMovement* movement = frame.Unsafe.GetPointer<PlayerCharacterMovement>(entityRef);
			PlayerCharacterMovementConfig config = frame.FindAsset<PlayerCharacterMovementConfig>(movement->MovementConfig.Id);

			movement->MovingSpeed = config.BasePlayerMovingSpeed;
		}

		public override void Update(Frame f, ref Filter filter)
		{
			GameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);

			Input* input = f.GetPlayerInput(filter.PlayerLink->PlayerRef);

			UpdateMovement(f, ref filter, input, config);
		}

		private void UpdateMovement(Frame f, ref Filter filter, Input* input, GameConfig config)
		{
			FPVector2 movingVector = input->Direction;

			filter.PhysicsBody->Velocity = movingVector * filter.PlayerCharacterMovement->MovingSpeed;

			if (!movingVector.Equals(FPVector2.Zero))
			{
				filter.Transform->Rotation = FPVector2.Radians(FPVector2.Up, movingVector) * -FPMath.Sign(movingVector.X);
			}
		}
	}
}