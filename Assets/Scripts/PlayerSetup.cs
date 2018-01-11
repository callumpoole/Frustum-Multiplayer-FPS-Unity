using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {
    [SerializeField]
    Behaviour[] componentsToDisable;

    Camera sceneCam;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    GameObject GUIPrefab;
    GameObject GUIInst;

    private uint networkID = 0;
    public uint GetNetworkID() { return networkID; }

    void Start() {
        if (!isLocalPlayer) {
            DisableComponents();
            AssignRemoteLayer();
        } else {
            sceneCam = Camera.main;
            if (sceneCam != null)
                sceneCam.gameObject.SetActive(false);
            GUIInst = Instantiate(GUIPrefab);
        }
        networkID = GetComponent<NetworkIdentity>().netId.Value;
        GetComponent<Player>().Setup();
    }

    public override void OnStartClient() {
        base.OnStartClient();
        GameManager.RegisterPlayer(GetComponent<NetworkIdentity>().netId.Value, GetComponent<Player>());
    }

    void AssignRemoteLayer() {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents() {
        for (int i = 0; i < componentsToDisable.Length; i++) {
            componentsToDisable[i].enabled = false;
        }
    }

    void OnDisable() {
        if (sceneCam != null)
            sceneCam.gameObject.SetActive(true);
        GameManager.UnRegisterPlayer(networkID);
        Destroy(GUIInst);
    }
}
