using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : MonoBehaviour
{
	[SerializeField]
	private RectTransform playerContent;
	[SerializeField]
	private PlayerEntry playerEntryPrefab;
	[SerializeField]
	private Button startButton;

	private List<PlayerEntry> playerEntryList;

	private void Awake()
	{
		playerEntryList = new List<PlayerEntry>();
	}

	public void UpdateRoomState()
	{
		foreach (PlayerEntry entry in playerEntryList)
		{
			Destroy(entry.gameObject);
		}
		playerEntryList.Clear();

		foreach (Player player in PhotonNetwork.PlayerList)
		{
			PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
			entry.Initailize(player.ActorNumber, player.NickName);
			object isPlayerReady;
			if (player.CustomProperties.TryGetValue(GameData.PLAYER_READY, out isPlayerReady))
			{
				entry.SetPlayerReady((bool)isPlayerReady);
			}
			playerEntryList.Add(entry);
		}

		if (!PhotonNetwork.IsMasterClient)
			return;

		startButton.gameObject.SetActive(CheckPlayerReady());
	}

	public bool CheckPlayerReady()
	{
		foreach (Player player in PhotonNetwork.PlayerList)
		{
			object isPlayerReady;
			if (player.CustomProperties.TryGetValue(GameData.PLAYER_READY, out isPlayerReady))
			{
				if (!(bool)isPlayerReady)
					return false;
			}
			else
			{
				return false;
			}
		}

		return true;
	}

	public void OnStartButtonClicked()
	{
		PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.CurrentRoom.IsVisible = false;

		PhotonNetwork.LoadLevel("GameScene");
	}

    public void OnLeaveRoomClicked()
	{
		PhotonNetwork.LeaveRoom();
	}
}
