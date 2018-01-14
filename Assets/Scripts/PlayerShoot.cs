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
	
    //Called wether of not anything is hit or not
    [Command]
    void CmdOnShoot() {
        RpcDoShootEffect();
    }

    [ClientRpc]
    void RpcDoShootEffect() { //When one person Shoots, show visually to all clients
        wm.GetCurrentWeaponGraphics().weaponGFX.Play();
    }

    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal) {
        RpcDoHitEffect(pos, normal);
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos, Vector3 normal) { //When one person Shoots, show visually to all clients
        //Could use Object Pooling for better efficiency
        GameObject hitGFX = Instantiate(wm.GetCurrentWeaponGraphics().hitEffectPrefab, pos, Quaternion.LookRotation(normal));
        Destroy(hitGFX, 2f);
    }

    [Client]
	void Shoot() {
        if (!isLocalPlayer)
            return;
        CmdOnShoot();

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask)) {
            if (hit.collider.tag == PLAYER_TAG) {
                CmdPlayerShot(hit.collider.GetComponent<PlayerSetup>().GetNetworkID(), currentWeapon.damage);
            }
            CmdOnHit(hit.point, hit.normal);
        }
    }

    [Command]
    void CmdPlayerShot(uint ID, int damage) {
        Debug.Log("Player " + ID + " has been shot");

        Player player = GameManager.GetPlayer(ID);
        player.RpcTakeDamage(damage);
    }
}
