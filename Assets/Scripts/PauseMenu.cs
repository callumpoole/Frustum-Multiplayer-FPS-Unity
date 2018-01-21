using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
public class PauseMenu : MonoBehaviour {
    public static bool isPaused = false;

    private NetworkManager networkManager;

    void Start() {
        networkManager = NetworkManager.singleton;
    }

    void Update() {
        if (isPaused && Input.GetKeyDown(KeyCode.K)) {
            LeaveRoom();
        }
    }

    public void LeaveRoom() {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }
}
