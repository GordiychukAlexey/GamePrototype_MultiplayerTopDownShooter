using TMPro;
using UnityEngine;
using Quantum;

namespace Game
{
	public unsafe class PlayerCharacterView : QuantumEntityViewComponent
	{
		public Canvas Canvas;
		public TextMeshProUGUI NameText;

		private Quaternion initialCanvasRotation;

		public override void OnActivate(Frame frame)
		{
			transform.localRotation = Quaternion.identity;

			initialCanvasRotation = Canvas.transform.rotation;

			var playerLink = PredictedFrame.Get<PlayerLink>(_entityView.EntityRef);
			var playerData = PredictedFrame.GetPlayerData(playerLink.PlayerRef);

			NameText.text = playerData.PlayerNickname;
		}

		public override void OnUpdateView()
		{
			Canvas.transform.rotation = initialCanvasRotation;      
		}
	}
}