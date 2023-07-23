using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] RectTransform roomContent;
    [SerializeField] RoomEntry roomEntryPrefab;

    Dictionary<string, RoomEntry> roomDictionary;

    private void Awake()
    {
        roomDictionary = new Dictionary<string, RoomEntry>();
    }

    private void OnDisable()
    {
        foreach (string key in roomDictionary.Keys)
        {
            Destroy(roomDictionary[key].gameObject);
        }
        roomDictionary.Clear();
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        // Update room info
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (info.RemovedFromList || !info.IsOpen || !info.IsVisible)
            {
                if (roomDictionary.ContainsKey(info.Name))
                {
                    Destroy(roomDictionary[info.Name].gameObject);
                    roomDictionary.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (roomDictionary.ContainsKey(info.Name))
            {
                roomDictionary[info.Name].SetRoomInfo(info);
            }
            // Add new room info to cache
            else
            {
                RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);
                entry.SetRoomInfo(info);
                roomDictionary.Add(info.Name, entry);
            }
        }
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }
}
