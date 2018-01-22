//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {


    [SerializeField]
    private float lookSensitivity = 3f;


    PlayerMotor motor;
    
	// Use this for initialization
	void Start () {
        motor = GetComponent<PlayerMotor>();
	}
	
	// Update is called once per frame
	void Update () {
        if (PauseMenu.isPaused)
            return;

        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");
        Vector3 hor = transform.right * xMov;
        Vector3 ver = transform.forward * zMov;

        Vector3 vel = (hor + ver).normalized;
        motor.Move(vel);

        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rot = new Vector3(0f, yRot, 0f) * lookSensitivity;
        motor.Rotate(rot);

        float xRot = Input.GetAxisRaw("Mouse Y");
        float camRot = xRot * lookSensitivity;
        motor.RotateCamera(camRot);

        if (Input.GetButtonDown("Jump"))
            motor.Jump();
        motor.IsHoldingJump(Input.GetButton("Jump"));
    }
}
