
using UnityEngine;

public class WeaponGraphics : MonoBehaviour {

    public ParticleSystem weaponGFX;
    public GameObject hitEffectPrefab;
    public Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }
}
