
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager inst;
    private bool showControls = true;

    public MatchSettings matchSettings; 
    public GameObject tempObjects;

    [SerializeField]
    private GameObject sceneCam;

    void Awake() {
        if(inst != null) {
            Debug.LogError("Multiple GameManager(s) in scene");
        } else {
            inst = this;
        }
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.H))
            showControls = !showControls;
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
        //OnPlayerJoin(netID, player);
    }

    public static Player GetPlayer(uint id) {
        return players[id];
    }

    public static Player[] GetAllPlayers() {
        Player[] ps = new Player[players.Count];
        players.Values.CopyTo(ps, 0);
        return ps;
    }

    public static void UnRegisterPlayer(uint netID) {
        Debug.Log("Unregistered Player:" + netID);
        players.Remove(netID);
    }


    #endregion

    #region GUI
    void OnGUI() {
        GUI.contentColor = Color.black;
        DrawAllGUI(new Rect(21, 21, 400, 500));
        GUI.contentColor = Color.white;
        DrawAllGUI(new Rect(20, 20, 400, 500)); 
    }
    void DrawAllGUI(Rect r) {
        GUILayout.BeginArea(r);
        GUILayout.BeginVertical();
        {
            if (showControls) {
                ControlsGUI();
                GUILayout.Label("---------");
            }
            LobbyStatusGUI();
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    void ControlsGUI() {
        GUILayout.Label("Pause - Esc");
        GUILayout.Label("Move - WASD");
        GUILayout.Label("Jump - Space");
        GUILayout.Label("Attack - Mouse1");
        GUILayout.Label("Block - Mouse2 (Coming soon)");
        GUILayout.Label("Change Weapon - Mouse Wheel (Coming soon)");
        GUILayout.Label("3rd Person - V");
        GUILayout.Label("Cursor Lock - C");
        GUILayout.Label("Hide These Controls - H");
    }
    void LobbyStatusGUI() {
        foreach (uint id in players.Keys) {
            GUILayout.Label(PLAYER_ID_PREFIX + id + "  -  " +
                players[id].transform.name + "  -  " +
                players[id].GetComponent<Player>().GetCurrentHealth());
        }
    }

    #endregion


    //private static void OnPlayerJoin(uint netID, Player player) {

    //}
}
