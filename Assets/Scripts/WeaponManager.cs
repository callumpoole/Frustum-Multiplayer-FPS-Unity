using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {
    [SerializeField]
    private const string WEAPON_LAYER = "WeaponLayer";

    [SerializeField]
    private Transform weaponHolder;
    [SerializeField]
    private Camera weaponCam;
    private GameObject weaponInst;
    [SerializeField]
    private PlayerWeapon defaultWeapon;
    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentWeaponGraphics;

	// Use this for initialization
	void Start () {
        EquipWeapon(defaultWeapon); 
    }
	
    public PlayerWeapon GetCurrentWeapon() {
        return currentWeapon;
    }
    public WeaponGraphics GetCurrentWeaponGraphics() {
        return currentWeaponGraphics;
    }

    void EquipWeapon(PlayerWeapon weapon) {
        currentWeapon = defaultWeapon;
        weaponInst = Instantiate(currentWeapon.GFX, weaponHolder.position + currentWeapon.offset, weaponHolder.rotation * currentWeapon.rotOffset);
        weaponInst.transform.SetParent(weaponHolder);

        currentWeaponGraphics = weaponInst.GetComponent<WeaponGraphics>();
        if (currentWeaponGraphics == null)
            Debug.LogError("No WeaponGraphics component on Weapon: " + weaponInst.name);

        if (currentWeapon.FOV > 0)
            weaponCam.fieldOfView = currentWeapon.FOV;
        if (isLocalPlayer)
            Util.recursivelyApplyLayer(weaponInst, LayerMask.NameToLayer(WEAPON_LAYER));
        
    }


}
