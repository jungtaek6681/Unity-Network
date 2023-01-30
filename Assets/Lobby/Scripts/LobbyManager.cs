using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
	public enum Panel { Login, InConnect, Lobby, Room }

	[SerializeField]
	private GameObject loginPanel;
	[SerializeField]
	private GameObject inConnectPanel;
	[SerializeField]
	private GameObject roomPanel;

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

		// TODO : ·ë ±¸Çö
	}

	public override void OnLeftRoom()
	{
		SetActivePanel(Panel.InConnect);
		AddMessage("Left room");
	}

	private void SetActivePanel(Panel panel)
	{
		loginPanel.SetActive(panel == Panel.Login);
		inConnectPanel.SetActive(panel == Panel.InConnect);
		roomPanel.SetActive(panel == Panel.Room);
	}

	private void AddMessage(string message)
	{
		StatePanel.Instance.AddMessage(message);
	}
}
