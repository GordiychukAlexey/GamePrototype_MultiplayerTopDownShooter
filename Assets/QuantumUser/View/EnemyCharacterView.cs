using Quantum;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public unsafe class EnemyCharacterView : QuantumEntityViewComponent
	{
		public Canvas Canvas;
		public Slider HealthSlider;

		private Quaternion initialCanvasRotation;

		public override void OnActivate(Frame frame)
		{
			transform.localRotation = Quaternion.identity;

			initialCanvasRotation = Canvas.transform.rotation;
		}

		public override void OnUpdateView()
		{
			Canvas.transform.rotation = initialCanvasRotation;

			var enemyHealth = PredictedFrame.Get<EnemyHealth>(_entityView.EntityRef);
			EnemyHealthConfig enemyHealthConfig = PredictedFrame.FindAsset<EnemyHealthConfig>(enemyHealth.EnemyHealthConfig.Id);
			HealthSlider.value = Mathf.Clamp01((enemyHealth.CurrentHealth/enemyHealthConfig.MaxHealth).AsFloat);
		}
	}
}