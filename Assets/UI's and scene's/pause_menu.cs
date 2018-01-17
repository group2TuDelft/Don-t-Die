using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauzemenu : MonoBehaviour {

    public static bool GameIsPauzed = false;
    public GameObject pauseMenuUI;

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPauzed)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }	
	}

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPauzed = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPauzed = true;
    }
}
