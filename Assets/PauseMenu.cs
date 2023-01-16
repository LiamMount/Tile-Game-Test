using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    GameMaster gm;

    //UI elements
    public GameObject pauseMenuUI;

    public GameObject arrow;

    public Transform resumeTransform;
    public Transform quitTransform;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameMaster>();

        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gm.bm.runningFight)
        {
            if (gm.gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (gm.gameIsPaused)
        {
            MenuUsage();
        }
    }

    public void MenuUsage()
    {
        //Arrow movement
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //On resume button, up to quit
            if (arrow.transform.position == resumeTransform.position)
            {
                arrow.transform.position = quitTransform.position;
            }
            //On quit button, up to resume
            else if (arrow.transform.position == quitTransform.position)
            {
                arrow.transform.position = resumeTransform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //On resume button, down to quit
            if (arrow.transform.position == resumeTransform.position)
            {
                arrow.transform.position = quitTransform.position;
            }
            //On quit button, down to resume
            else if (arrow.transform.position == quitTransform.position)
            {
                arrow.transform.position = resumeTransform.position;
            }
        }

        //Selection
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Resume Button Select
            if (arrow.transform.position == resumeTransform.position)
            {
                Resume();
            }

            //Quit Button Select
            else if (arrow.transform.position == quitTransform.position)
            {
                // Put quit function here later
            }
        }
        //Maybe put back in later
        /*
        //De-selection
        if (Input.GetKeyDown(KeyCode.X))
        {
            Resume();
        }
        */
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gm.gameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gm.gameIsPaused = true;
    }
}
