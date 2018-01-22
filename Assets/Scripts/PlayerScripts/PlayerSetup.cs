using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    Material[] playerSkins;
   

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    GameObject GUIPrefab;
    [HideInInspector]
    public GameObject GUIInst;

    private uint networkID = 0;
    public uint GetNetworkID() { return networkID; }

    void Start() {
        if (!isLocalPlayer) {
            DisableComponents();
            AssignRemoteLayer();
        } else {
            GUIInst = Instantiate(GUIPrefab);
            GUIInst.name = GUIPrefab.name; 
            GetComponent<Player>().SetupPlayer();
        }
        networkID = GetComponent<NetworkIdentity>().netId.Value;
        CmdSetupPlayerColour();
    }

    public override void OnStartClient() {
        base.OnStartClient();
        GameManager.RegisterPlayer(GetComponent<NetworkIdentity>().netId.Value, GetComponent<Player>()); 
    }

    void AssignRemoteLayer() {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    [Command]
    void CmdSetupPlayerColour() {
        RpcSetupPlayerColour();
    }
    [ClientRpc]
    void RpcSetupPlayerColour() {
        foreach (Player p in GameManager.GetAllPlayers()) {
            uint pid = p.GetComponent<PlayerSetup>().networkID;
            if (pid == 0)
                continue;
            foreach (Transform part in p.transform.Find("CharacterModel").Find("Body")) {
                part.GetComponent<MeshRenderer>().material = playerSkins[pid - 1];
            }
        }

    }


    void DisableComponents() {
        for (int i = 0; i < componentsToDisable.Length; i++) {
            componentsToDisable[i].enabled = false;
        }
    } 
    void OnDisable() {
        if (isLocalPlayer)
            GameManager.inst.SetSceneCameraActive(true);
        GameManager.UnRegisterPlayer(networkID);
        Destroy(GUIInst);
    }
}
