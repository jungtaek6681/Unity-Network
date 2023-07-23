using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, Menu, Lobby, Room }

    [SerializeField] StatePanel statePanel;

    [SerializeField] LoginPanel loginPanel;
    [SerializeField] MenuPanel menuPanel;
    [SerializeField] RoomPanel roomPanel;
    [SerializeField] LobbyPanel lobbyPanel;

    public void Start()
    {
        SetActivePanel(Panel.Login);
    }

    public override void OnConnectedToMaster()
    {
        SetActivePanel(Panel.Menu);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SetActivePanel(Panel.Login);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
        Debug.Log($"Create room failed with error({returnCode}) : {message}");
        statePanel.AddMessage($"Create room failed with error({returnCode}) : {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
        Debug.Log($"Join room failed with error({returnCode}) : {message}");
        statePanel.AddMessage($"Join room failed with error({returnCode}) : {message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
        Debug.Log($"Join random room failed with error({returnCode}) : {message}");
        statePanel.AddMessage($"Join random room failed with error({returnCode}) : {message}");
    }

    public override void OnJoinedRoom()
    {
        SetActivePanel(Panel.Room);

        // TODO : Room ±¸Çö
    }

    public override void OnLeftRoom()
    {
        SetActivePanel(Panel.Menu);
    }

    public override void OnJoinedLobby()
    {
        SetActivePanel(Panel.Lobby);
    }

    public override void OnLeftLobby()
    {
        SetActivePanel(Panel.Menu);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        lobbyPanel.UpdateRoomList(roomList);
    }

    private void SetActivePanel(Panel panel)
    {
        if (loginPanel != null) loginPanel.gameObject.SetActive(panel == Panel.Login);
        if (menuPanel != null) menuPanel.gameObject.SetActive(panel == Panel.Menu);
        if (roomPanel != null) roomPanel.gameObject.SetActive(panel == Panel.Room);
        if (lobbyPanel != null) lobbyPanel.gameObject.SetActive(panel == Panel.Lobby);
    }
}
