using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private TMP_Text infoText;

	private void Start()
	{
		if (PhotonNetwork.InRoom)
		{
			Hashtable props = new Hashtable() { { GameData.PLAYER_LOAD, true } };
			PhotonNetwork.LocalPlayer.SetCustomProperties(props);
		}
		else // 게임씬 테스트용 빠른 접속
		{
			PhotonNetwork.ConnectUsingSettings();
			infoText.text = "";
		}
	}

	#region PUN Callback

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions() { MaxPlayers = 8 }, null);
	}

	public override void OnJoinedRoom()
	{
		StartCoroutine(TestGameDelay());
	}



	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log(string.Format("Disconnected : {0}", cause.ToString()));
		SceneManager.LoadScene("LobbyScene");
	}

	public override void OnLeftRoom()
	{
		Debug.Log("OnLeftRoom");
		SceneManager.LoadScene("LobbyScene");
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		if (changedProps.ContainsKey(GameData.PLAYER_LOAD))
		{
			if (CheckAllPlayerLoadLevel())
			{
				StartCoroutine(StartCountDown());
			}
			else
			{
				PrintInfo("wait players " + PlayersLoadLevel() + " / " + PhotonNetwork.PlayerList.Length);
			}
		}
	}

	#endregion

	private void GameStart()
	{
		float angularStart = (360.0f / PhotonNetwork.CurrentRoom.PlayerCount) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
		float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
		float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
		Vector3 position = new Vector3(x, 0.0f, z);
		Quaternion rotation = Quaternion.Euler(0.0f, angularStart, 0.0f);

		PhotonNetwork.Instantiate("Player", position, rotation, 0);
	}

	private void TestGameStart()
	{
		float angularStart = (360.0f / PhotonNetwork.CurrentRoom.PlayerCount) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
		float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
		float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
		Vector3 position = new Vector3(x, 0.0f, z);
		Quaternion rotation = Quaternion.Euler(0.0f, angularStart, 0.0f);

		PhotonNetwork.Instantiate("Player", position, rotation, 0);
	}

	private IEnumerator StartCountDown()
	{
		PrintInfo("All Player Loaded, Start Count Down");
		yield return new WaitForSeconds(1.0f);

		for (int i = GameData.COUNTDOWN; i > 0; i--)
		{
			PrintInfo("Count Down " + i);
			yield return new WaitForSeconds(1.0f);
		}

		PrintInfo("Start Game!");
		GameStart();

		yield return new WaitForSeconds(1f);
		infoText.text = "";
	}

	private IEnumerator TestGameDelay()
	{
		yield return new WaitForSeconds(1.0f);
		TestGameStart();
	}

	private bool CheckAllPlayerLoadLevel()
	{
		return PlayersLoadLevel() == PhotonNetwork.PlayerList.Length;
	}

	private int PlayersLoadLevel()
	{
		int count = 0;
		foreach (Player p in PhotonNetwork.PlayerList)
		{
			object playerLoadedLevel;
			if (p.CustomProperties.TryGetValue(GameData.PLAYER_LOAD, out playerLoadedLevel))
			{
				if ((bool)playerLoadedLevel)
				{
					count++;
				}
			}
		}

		return count;
	}

	private void PrintInfo(string info)
	{
		Debug.Log(info);
		infoText.text = info;
	}
}
