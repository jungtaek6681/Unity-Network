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

	public override void OnConnectedToMaster()
	{
		SetActivePanel(Panel.InConnect);
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		SetActivePanel(Panel.Login);
	}

	private void SetActivePanel(Panel panel)
	{
		loginPanel.SetActive(panel == Panel.Login);
		inConnectPanel.SetActive(panel == Panel.InConnect);
	}
}
