using Quantum;
using UnityEngine.Scripting;

namespace Game
{
	[Preserve]
	public unsafe class PlayerSystem : SystemSignalsOnly, ISignalOnPlayerConnected, ISignalOnPlayerDisconnected
	{
		public void OnPlayerConnected(Frame frame, PlayerRef player)
		{
			RuntimePlayer data = frame.GetPlayerData(player);

			var playerAvatarAssetRef = data.PlayerAvatar.IsValid
				? data.PlayerAvatar
				: frame.RuntimeConfig.PlayerCharacterPrefab;
			var playerCharacterPrototypeAsset = frame.FindAsset(playerAvatarAssetRef);
			var playerCharacterEntity = frame.Create(playerCharacterPrototypeAsset);
			frame.Set(playerCharacterEntity, new PlayerLink { PlayerRef = player });

			frame.Signals.OnSpawnPlayerCharacter(playerCharacterEntity);
		}

		public void OnPlayerDisconnected(Frame f, PlayerRef player)
		{
			ComponentFilter<PlayerLink> filter = f.Filter<PlayerLink>();
			while (filter.Next(out EntityRef e, out PlayerLink link))
			{
				if (link.PlayerRef == player)
				{
					f.Destroy(e);
				}
			}
		}
	}
}