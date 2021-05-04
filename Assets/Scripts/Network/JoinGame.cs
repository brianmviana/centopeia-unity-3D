using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System;

public class JoinGame : MonoBehaviour {

    List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    private TextMeshProUGUI status;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private Transform roomListParent;


    private NetworkManager networkManager;

    private void Start() {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null) {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();
    }

    public void RefreshRoomList() {
        ClearRoomList();
        networkManager.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);
        status.text = "Loading ...";
    }

    private void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches) {
        status.text = "";
        if (!success || matches == null) {
            status.text = "Couldn't get room list.";
            return;
        }

        ClearRoomList();

        foreach (MatchInfoSnapshot match in matches) {
            GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
            _roomListItemGO.transform.SetParent(roomListParent);

            RoomListItem _roomListItem = _roomListItemGO.GetComponent<RoomListItem>();
            if (_roomListItem != null) {
                _roomListItem.Setup(match, JoinRoom);
            }

            roomList.Add(_roomListItemGO);
        }

        if (roomList.Count == 0) {
            status.text = "No room at the moment...";
        }

    }

    void ClearRoomList() {
        for (int i = 0; i < roomList.Count; i++) {
            Destroy(roomList[i]);
        }
        roomList.Clear();
    }

    public void JoinRoom(MatchInfoSnapshot _match) {
        Debug.Log("Joining " + _match.name);

        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        ClearRoomList();
        status.text = "JOINING ...";
    }

}
