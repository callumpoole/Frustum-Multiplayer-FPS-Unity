using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWeapon {

    public string name = "Fists";

    public int damage = 30;
    public float range = 100f;
    public float fireRate = 0f;

    public GameObject GFX;
    public Vector3 offset;
    public Quaternion rotOffset;
    public float FOV;
}
