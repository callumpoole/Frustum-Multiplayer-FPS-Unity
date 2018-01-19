
using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint roomSize = 4;
    private string roomName;
    private string roomPass = "";
    private NetworkManager networkManager;

    void Start() {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null) {
            networkManager.StartMatchMaker();
        }
    }
    public void SetRoomName (string name) {
        roomName = name;
    }
    public void SetRoomPassword(string pass) {
        roomPass = pass;
    }
    public void CreateRoom() {
        if (roomName != "" && roomName != null) {
            Debug.Log("Creating Room: " + roomName + " with space for: " + roomSize + " players.");
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, roomPass, "" , "", 0, 0, networkManager.OnMatchCreate);
        }
    }
}
