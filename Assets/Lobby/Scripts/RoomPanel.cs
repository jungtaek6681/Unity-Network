using Photon.Pun;
using UnityEngine;

public class RoomPanel : MonoBehaviour
{
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
