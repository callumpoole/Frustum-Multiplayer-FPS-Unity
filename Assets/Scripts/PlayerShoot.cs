using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private PlayerWeapon weapon;
    [SerializeField]
    private GameObject weaponGFX;
    [SerializeField]
    private const string WEAPON_LAYER = "WeaponLayer";

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask mask;

    void Start() {
        if (cam == null) {
            Debug.LogError("PlayerShoot: No Camera Referenced");
            this.enabled = false;
        }
        weaponGFX.layer = LayerMask.NameToLayer(WEAPON_LAYER);
        foreach (Transform t in weaponGFX.transform)
            t.gameObject.layer = LayerMask.NameToLayer(WEAPON_LAYER);
    }


	void Update () {
		if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
	}
	
    [Client]
	void Shoot() {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask)) {
            if (hit.collider.tag == PLAYER_TAG) {
                CmdPlayerShot(hit.collider.GetComponent<PlayerSetup>().GetNetworkID(), weapon.damage);
            }
        }
    }

    [Command]
    void CmdPlayerShot(uint ID, int damage) {
        Debug.Log("Player " + ID + " has been shot");

        Player player = GameManager.GetPlayer(ID);
        player.RpcTakeDamage(damage);
    }
}
