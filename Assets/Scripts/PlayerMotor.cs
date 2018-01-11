//using System.Collections;
//using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Camera cam_3rd;


    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currecntCameraRotationX = 0f;
    [SerializeField]
    private float cameraRotationLimit = 85f;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}

    public void Move(Vector3 vel) {
        velocity = vel;
    }
    public void Rotate(Vector3 rot) {
        rotation = rot;
    }
    public void RotateCamera(float camRotX) { 
        cameraRotationX = camRotX;
    }

    // Update is called once per Tick
    void FixedUpdate () {
        PerformMovement();
        PerformRotation();
    }

    void PerformMovement() {
        if (velocity != Vector3.zero) {
            rb.MovePosition(rb.position + velocity * Time.deltaTime);
        }
    }

    void PerformRotation() {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null) {
            currecntCameraRotationX -= cameraRotationX;
            currecntCameraRotationX = Mathf.Clamp(currecntCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
            cam.transform.localEulerAngles = new Vector3(currecntCameraRotationX, 0f, 0f);

        }
    }


}
