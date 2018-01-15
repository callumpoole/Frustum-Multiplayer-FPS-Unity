
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager inst;

    public MatchSettings matchSettings;

    [SerializeField]
    private GameObject sceneCam;

    void Awake() {
        if(inst != null) {
            Debug.LogError("Multiple GameManager(s) in scene");
        } else {
            inst = this;
        }
    }

    public void SetSceneCameraActive(bool isActive) {
        if (sceneCam == null)
            return;
        sceneCam.SetActive(isActive);
    }

    #region Player Tracking
    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<uint, Player> players = new Dictionary<uint, Player>();

    public static void RegisterPlayer(uint netID, Player player) {
        string playerID = PLAYER_ID_PREFIX + netID;
        players.Add(netID, player);
        player.transform.name = playerID;
        Debug.Log("Registered Player:" + playerID);
    }

    public static Player GetPlayer(uint id) {
        return players[id];
    }

    public static void UnRegisterPlayer(uint netID) {
        Debug.Log("Unregistered Player:" + netID);
        players.Remove(netID);
    }

    void OnGUI() {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();
        foreach(uint id in players.Keys) {
            GUILayout.Label(PLAYER_ID_PREFIX + id + "  -  " + 
                players[id].transform.name + "  -  " + 
                players[id].GetComponent<Player>().GetCurrentHealth());
        }
        GUILayout.EndArea();
        GUILayout.EndVertical();
    }
    #endregion


}
