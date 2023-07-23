using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, Menu, Lobby, Room }

    [SerializeField] LoginPanel loginPanel;
    [SerializeField] MenuPanel menuPanel;

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

    private void SetActivePanel(Panel panel)
    {
        if (loginPanel != null) loginPanel.gameObject.SetActive(panel == Panel.Login);
        if (menuPanel != null) menuPanel.gameObject.SetActive(panel == Panel.Menu);
    }
}
