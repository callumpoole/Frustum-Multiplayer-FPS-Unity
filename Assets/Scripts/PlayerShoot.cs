using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager wm;

    void Start() {
        if (cam == null) {
            Debug.LogError("PlayerShoot: No Camera Referenced");
            this.enabled = false;
        }
        //weaponGFX.layer = LayerMask.NameToLayer(WEAPON_LAYER);
        //foreach (Transform t in weaponGFX.transform)
        //    t.gameObject.layer = LayerMask.NameToLayer(WEAPON_LAYER);
        wm = GetComponent<WeaponManager>();
    }


    void Update() {
        currentWeapon = wm.GetCurrentWeapon();
        if (currentWeapon.fireRate <= 0) {
            if (Input.GetButtonDown("Fire1")) {
                Shoot();
            }
        } else {
            if (Input.GetButtonDown("Fire1")) {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
            } else if (Input.GetButtonUp("Fire1")) {
                CancelInvoke("Shoot");
            }
        }
    }
	
    [Client]
	void Shoot() {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask)) {
            if (hit.collider.tag == PLAYER_TAG) {
                CmdPlayerShot(hit.collider.GetComponent<PlayerSetup>().GetNetworkID(), currentWeapon.damage);
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
