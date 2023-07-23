using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text currentPlayer;
    [SerializeField] Button joinRoomButton;

    private RoomInfo info;

    public void SetRoomInfo(RoomInfo info)
    {
        this.info = info;
        roomName.text = info.Name;
        currentPlayer.text = $"{info.PlayerCount} / {info.MaxPlayers}";
        joinRoomButton.interactable = info.PlayerCount < info.MaxPlayers;
    }

    public void JoinRoom()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinRoom(info.Name);
    }
}
