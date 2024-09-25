using Quantum;
using TMPro;
using Text = TMPro.TextMeshProUGUI;

namespace Game
{
	public unsafe class PlayerHud : QuantumSceneViewComponent
	{
		public TextMeshProUGUI ScoreLabel;
		public TextMeshProUGUI MovingSpeedLabel;
		public TextMeshProUGUI AttackRadiusLabel;
		public TextMeshProUGUI DamagePerSecondLabel;

		public override void OnUpdateView()
		{
			var shipsFilter = VerifiedFrame.Filter<PlayerLink, PlayerCharacter>();

			while (shipsFilter.Next(out var entity, out var playerLink, out var playerCharacter))
			{
				if (QuantumRunner.Default.Game.PlayerIsLocal(playerLink.PlayerRef))
				{
					var score = PredictedFrame.Get<PlayerCharacterScore>(entity);
					var movement = PredictedFrame.Get<PlayerCharacterMovement>(entity);
					var attack = PredictedFrame.Get<PlayerCharacterAttack>(entity);

					ScoreLabel.text = $"Score: {score.Kills}";
					MovingSpeedLabel.text = $"MovingSpeed: {movement.MovingSpeed.AsFloat}";
					AttackRadiusLabel.text = $"AttackRadius: {attack.AttackRadius.AsFloat}";
					DamagePerSecondLabel.text = $"DamagePerSecond: {attack.DamagePerSecond.AsFloat}";
				}
			}
		}
	}
}