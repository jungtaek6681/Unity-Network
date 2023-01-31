using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerEntry : MonoBehaviour
{
	[SerializeField]
	private TMP_Text playerName;
	[SerializeField]
	private TMP_Text playerReady;
	[SerializeField]
	private Button playerReadyButton;

	private int ownerId;

	public void Initailize(int id, string name)
	{
		ownerId = id;
		if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
		{
			playerReadyButton.gameObject.SetActive(false);
		}
		playerName.text = name;
		playerReady.text = "";
	}

	public void SetPlayerReady(bool ready)
	{
		playerReady.text = ready ? "Ready" : "";
	}

	public void OnReadyButtonClicked()
	{
		object isPlayerReady;
		if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(GameData.PLAYER_READY, out isPlayerReady))
			isPlayerReady = false;

		bool ready = (bool)isPlayerReady;
		SetPlayerReady(!ready);

		ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { GameData.PLAYER_READY, !ready } };
		PhotonNetwork.LocalPlayer.SetCustomProperties(props);
	}
}
