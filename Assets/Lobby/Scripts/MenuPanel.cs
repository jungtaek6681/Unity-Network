using Photon.Pun;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public void Logout()
    {
        PhotonNetwork.Disconnect();
    }
}
