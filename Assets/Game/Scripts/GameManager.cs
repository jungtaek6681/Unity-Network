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

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
		{
			StartCoroutine(SpawnAsteroid());
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

		StartCoroutine(SpawnAsteroid());
	}

	private void TestGameStart()
	{
		float angularStart = (360.0f / PhotonNetwork.CurrentRoom.PlayerCount) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
		float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
		float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
		Vector3 position = new Vector3(x, 0.0f, z);
		Quaternion rotation = Quaternion.Euler(0.0f, angularStart, 0.0f);

		PhotonNetwork.Instantiate("Player", position, rotation, 0);

		StartCoroutine(SpawnAsteroid());
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

	private IEnumerator SpawnAsteroid()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(3, 5));

			Vector2 direction = Random.insideUnitCircle;
			Vector3 position = Vector3.zero;

			if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
			{
				// Make it appear on the left/right side
				position = new Vector3(Mathf.Sign(direction.x) * Camera.main.orthographicSize * Camera.main.aspect, 0, direction.y * Camera.main.orthographicSize);
			}
			else
			{
				// Make it appear on the top/bottom
				position = new Vector3(direction.x * Camera.main.orthographicSize * Camera.main.aspect, 0, Mathf.Sign(direction.y) * Camera.main.orthographicSize);
			}

			// Offset slightly so we are not out of screen at creation time (as it would destroy the asteroid right away)
			position -= position.normalized * 0.1f;


			Vector3 force = -position.normalized * 1000.0f;
			Vector3 torque = Random.insideUnitSphere * Random.Range(100.0f, 300.0f);
			object[] instantiationData = { force, torque };

			if (Random.Range(0, 10) < 5)
			{
				PhotonNetwork.InstantiateRoomObject("BigStone", position, Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f), 0, instantiationData);
			}
			else
			{
				PhotonNetwork.InstantiateRoomObject("SmallStone", position, Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f), 0, instantiationData);
			}
		}
	}
}
