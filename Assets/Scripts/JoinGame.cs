using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections.Generic;
using System.Collections;

public class JoinGame : MonoBehaviour {

    const int WAIT_FOR_JOIN_COUNTDOWN = 10;

    List<GameObject> roomList = new List<GameObject>();
    private NetworkManager networkManager;
    [SerializeField]
    private Text status;
    [SerializeField]
    private GameObject roomListItemPrefab;
    [SerializeField]
    private GameObject roomListParent;

    void Start () {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null) 
            networkManager.StartMatchMaker(); 
        RefreshRoomList();
	}



    public void RefreshRoomList() {
        ClearRoomList();
        if (networkManager.matchMaker == null) 
            networkManager.StartMatchMaker(); 
        networkManager.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);
        status.text = "Loading...";
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches) {
        status.text = "";
        if (!success) {
            status.text = "ERROR: Couldn't get room list.";
            return;
        }  
        if (matches.Count == 0) //No matches found
            status.text = "No Matches Found.";
        
        foreach (MatchInfoSnapshot match in matches) {
            GameObject roomListItemGO = Instantiate(roomListItemPrefab);
            roomListItemGO.transform.SetParent(roomListParent.transform);

            RoomListItem roomItem = roomListItemGO.GetComponent<RoomListItem>();
            if (roomItem != null)
                roomItem.Setup(match, JoinRoom);

            roomList.Add(roomListItemGO);
        } 
    }

    void ClearRoomList() {
        for (int i = 0; i < roomList.Count; i++) 
            Destroy(roomList[i]);
        roomList.Clear();
      
    }

    public void JoinRoom(MatchInfoSnapshot match) {
        networkManager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        StartCoroutine(WaitForJoin());
    }

    IEnumerator WaitForJoin() {
        ClearRoomList();
        int countDown = WAIT_FOR_JOIN_COUNTDOWN;
        while (countDown > 0) {
            status.text = "Joining... ("+countDown+")";
            yield return new WaitForSeconds(1);
            countDown--;
        }
        status.text = "Failed to Connect.";
        MatchInfo match = networkManager.matchInfo;
        if (match != null) {
            MatchInfo matchInfo = networkManager.matchInfo;
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();
        }
        RefreshRoomList();
    }
   
}
