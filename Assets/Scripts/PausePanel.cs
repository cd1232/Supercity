using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour {

    public GameObject PausePanelObject;
    bool bPause = false;
    bool bKeyDown = false;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bPause = !bPause;
            if (!bPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
	}

    // PAUSE MENU
    public void Pause()
    {
        bPause = true;
        PausePanelObject.SetActive(true);
        Time.timeScale = 0;

    }

    public void Resume()
    {
        bPause = false;
        PausePanelObject.SetActive(false);
        Time.timeScale = 1;
    }
}
