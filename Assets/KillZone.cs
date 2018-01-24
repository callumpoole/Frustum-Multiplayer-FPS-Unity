using UnityEngine;
using UnityEngine.Networking;

public class KillZone : NetworkBehaviour {

    void OnTriggerEnter(Collider c) {
        if (c.tag == "Player") {
            CmdKill(c.GetComponent<PlayerSetup>().GetNetworkID());
        }
    }

    [Command]
    void CmdKill(uint ID) {
        Debug.Log("Player " + ID + " was killed by the kill zone");

        Player player = GameManager.GetPlayer(ID);
        player.RpcTakeDamage(999999);
    }
}
