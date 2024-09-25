using Quantum;
using Photon.Deterministic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
	public class InputUpdater : MonoBehaviour
	{
		[SerializeField] private PlayerInput _playerInput;

		private void OnEnable()
		{
			QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
		}

		public void PollInput(CallbackPollInput callback)
		{
			Quantum.Input input = new();

			if (callback.Game.GetLocalPlayers().Count == 0)
			{
				return;
			}

			input.Direction = _playerInput.actions["Move"].ReadValue<Vector2>().ToFPVector2();
			input.Upgrade = _playerInput.actions["Upgrade"].IsPressed();

			callback.SetInput(input, DeterministicInputFlags.Repeatable);
		}
	}
}