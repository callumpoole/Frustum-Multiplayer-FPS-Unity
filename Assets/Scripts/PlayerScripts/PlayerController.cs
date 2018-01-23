//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {


    [SerializeField]
    private float lookSensitivity = 3f;

    [SerializeField]
    private KeyCode changeCamKey = KeyCode.V;
    public bool isFirstCam { get; private set; }    //Set as TRUE in START()
    [SerializeField]
    private Camera cam1st;
    [SerializeField]
    private Camera cam3rd;
    [SerializeField]
    private Camera camWeapon;
    private PlayerGUI pGUI = null;

    [SerializeField]
    private KeyCode toggleCursorLock = KeyCode.C;
    private bool shouldLockCursor = true;

    PlayerMotor motor;
    
	// Use this for initialization
	void Start () {
        motor = GetComponent<PlayerMotor>();
        isFirstCam = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (PauseMenu.isPaused) {
            if (Cursor.lockState != CursorLockMode.None)
                Cursor.lockState = CursorLockMode.None;
            motor.Move(Vector3.zero);
            motor.Rotate(Vector3.zero);
            motor.RotateCamera(0f);
            return;
        }
        if (shouldLockCursor && Cursor.lockState != CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.Locked;
        else if (!shouldLockCursor)
            Cursor.lockState = CursorLockMode.None;

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

        if (Input.GetKeyDown(changeCamKey)) {
            if (pGUI == null)
                pGUI = GetComponent<PlayerSetup>().GUIInst.GetComponent<PlayerGUI>();
            isFirstCam = !isFirstCam;         //Toggle First Person Bool
            cam1st.enabled = isFirstCam;            //Toggle 1st Cam
            camWeapon.enabled = isFirstCam;         //Toggle Weapon Cam
            cam3rd.enabled = !isFirstCam;           //Toggle 3rd cam
            pGUI.SettingTo1stPerson(isFirstCam);    //Toggle GUI appropriately
        }

        if (Input.GetKeyDown(toggleCursorLock))
            shouldLockCursor = !shouldLockCursor;
    }
}

