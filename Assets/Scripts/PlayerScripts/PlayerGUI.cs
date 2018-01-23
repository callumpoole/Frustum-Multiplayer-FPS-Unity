using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGUI : MonoBehaviour {

    [SerializeField] 
    GameObject pauseMenu;

    [SerializeField]
    private GameObject[] hideWhen3rdPerson;
    [SerializeField]
    private GameObject[] showWhen3rdPerson;

    public void SettingTo1stPerson(bool _1st) {
        foreach (GameObject g in hideWhen3rdPerson)
            g.SetActive(!_1st);
        foreach (GameObject g in showWhen3rdPerson)
            g.SetActive(_1st); 
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)){
            TogglePauseMenu();
        }
	}

    void TogglePauseMenu() {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isPaused = !PauseMenu.isPaused;
    }
}
