using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
	public enum Panel { Login, InConnect, Lobby, Room }

	[SerializeField]
	private LoginPanel loginPanel;
	[SerializeField]
	private InConnectPanel inConnectPanel;
	[SerializeField]
	private RoomPanel roomPanel;
	[SerializeField]
	private LobbyPanel lobbyPanel;

	private void Start()
	{
		if (PhotonNetwork.IsConnected)
		{
			OnConnectedToMaster();
		}
		else if (PhotonNetwork.InRoom)
		{
			OnJoinedRoom();
		}
		else if (PhotonNetwork.InLobby)
		{
			OnJoinedLobby();
		}
		else
		{
			OnDisconnected(DisconnectCause.None);
		}
	}

	public override void OnConnectedToMaster()
	{
		SetActivePanel(Panel.InConnect);
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		SetActivePanel(Panel.Login);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		SetActivePanel(Panel.InConnect);
		AddMessage(string.Format("Create room failed with error({0}) : {1}", returnCode, message));
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		SetActivePanel(Panel.InConnect);
		AddMessage(string.Format("Join room failed with error({0}) : {1}", returnCode, message));
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		SetActivePanel(Panel.InConnect);
		AddMessage(string.Format("Join random room failed with error({0}) : {1}", returnCode, message));
	}

	public override void OnJoinedRoom()
	{
		SetActivePanel(Panel.Room);
		AddMessage("Join room");

		Hashtable props = new Hashtable
		{
			{ GameData.PLAYER_READY, false }, 
			{ GameData.PLAYER_LOAD, false }
		};
		PhotonNetwork.LocalPlayer.SetCustomProperties(props);

		PhotonNetwork.AutomaticallySyncScene = true;
		roomPanel.UpdateRoomState();
	}

	public override void OnLeftRoom()
	{
		SetActivePanel(Panel.InConnect);
		AddMessage("Left room");
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		roomPanel.UpdateRoomState();
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		roomPanel.UpdateRoomState();
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		roomPanel.UpdateRoomState();
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		roomPanel.UpdateRoomState();
	}

	public override void OnJoinedLobby()
	{
		SetActivePanel(Panel.Lobby);
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		lobbyPanel.UpdateRoomList(roomList);
	}

	public override void OnLeftLobby()
	{
		SetActivePanel(Panel.InConnect);
	}

	private void SetActivePanel(Panel panel)
	{
		loginPanel.gameObject?.SetActive(panel == Panel.Login);
		inConnectPanel.gameObject?.SetActive(panel == Panel.InConnect);
		roomPanel.gameObject?.SetActive(panel == Panel.Room);
		lobbyPanel.gameObject?.SetActive(panel == Panel.Lobby);
	}

	private void AddMessage(string message)
	{
		StatePanel.Instance.AddMessage(message);
	}
}
