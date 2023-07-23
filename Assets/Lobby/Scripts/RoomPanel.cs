using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] RectTransform playerContent;
    [SerializeField] PlayerEntry playerEntryPrefab;
    [SerializeField] Button startButton;

    private Dictionary<int, PlayerEntry> playerDictionary;

    private void Awake()
    {
        playerDictionary = new Dictionary<int, PlayerEntry>();
    }

    private void OnEnable()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
            entry.SetPlayer(player);
            playerDictionary.Add(player.ActorNumber, entry);
        }
    }

    private void OnDisable()
    {
        foreach (int actorNumber in playerDictionary.Keys)
        {
            Destroy(playerDictionary[actorNumber].gameObject);
        }
        playerDictionary.Clear();
    }

    public void PlayerEnterRoom(Player newPlayer)
    {
        PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
        entry.SetPlayer(newPlayer);
        playerDictionary.Add(newPlayer.ActorNumber, entry);
    }

    public void PlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerDictionary[otherPlayer.ActorNumber].gameObject);
        playerDictionary.Remove(otherPlayer.ActorNumber);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
