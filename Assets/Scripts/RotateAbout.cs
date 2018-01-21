using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAbout : MonoBehaviour {

    [SerializeField]
    private bool shouldRotate = true;
    [SerializeField]
    private Vector3 vec;
    [SerializeField]
    private float speed = 1;
	

	void FixedUpdate () {
		if (shouldRotate) {
            transform.rotation *= Quaternion.Euler(vec*speed);
        }
	}
}
