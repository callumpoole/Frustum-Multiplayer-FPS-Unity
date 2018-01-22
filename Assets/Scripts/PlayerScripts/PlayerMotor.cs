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
    private bool shouldJump = false;
    private bool isHoldingJump = false;

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float jumpVel = 5f;
    [SerializeField]
    private float fallMultiplier = 2.5f;
    [SerializeField]
    private float lowJumpMultiplier = 2f;

    private Rigidbody rb;
    public bool isGrounded;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }
     
    void OnCollisionStay(Collision collisionInfo) {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collisionInfo) {
        isGrounded = false;
    }

    //public bool IsGrounded() {
    //    return Physics.Raycast(transform.position, -Vector3.up, 1<<12);
    //}

    public void Move(Vector3 vel) {
        velocity = vel * speed;
    }
    public void Rotate(Vector3 rot) {
        rotation = rot;
    }
    public void RotateCamera(float camRotX) { 
        cameraRotationX = camRotX;
    }
    public void Jump() {
        if (isGrounded)
            shouldJump = true; 
    }
    public void IsHoldingJump(bool ihj) {
        isHoldingJump = ihj;
    }

    // Update is called once per Tick
    void FixedUpdate () {
        PerformMovement();
        PerformRotation();
        PerformJump();
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

    void PerformJump() {
        if (shouldJump) {
            rb.velocity = Vector3.up * jumpVel;
            shouldJump = false;
        }
        if (rb.velocity.y < 0) {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !isHoldingJump) {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    } 
}
