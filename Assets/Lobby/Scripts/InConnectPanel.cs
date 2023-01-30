using Photon.Pun;
using UnityEngine;

public class InConnectPanel : MonoBehaviour
{
    public void OnLogoutButtonClicked()
	{
		PhotonNetwork.Disconnect();
	}
}
