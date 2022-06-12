using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{

    private static bool gameIsPaused = false;

    [Header("Element UI")]
    public GameObject pause;
    public GameObject resume;
    public GameObject panel;
    public GameObject stamina;
    public GameObject settings;

    [Header ("Audio")]
    public AudioSource select;
    public AudioSource selectBack;

    public static bool m_timeToStop;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused();
        }
    }

    private void Start()
    {
        gameIsPaused = false;

        Time.timeScale = 1f;

        Cursor.visible = false;
        stamina.SetActive(true);
        settings.SetActive(false);
        pause.SetActive(false);
        gameIsPaused = false;
        panel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;


    }
    /*
    public void resuming()
    {
        Cursor.visible = false;
        Time.timeScale = 0f;
        pause.SetActive(false);
        panel.SetActive(false);
        gameIsPaused = false;
    }
    */
    public void isPaused()
    {
        gameIsPaused = !gameIsPaused;
        PauseGame();
    }

    private void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            m_timeToStop = true;

            Cursor.visible = true;
            stamina.SetActive(false);
            pause.SetActive(true);
            gameIsPaused = true;
            panel.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Time.timeScale = 1f;
            m_timeToStop = false;

            Cursor.visible = false;
            stamina.SetActive(true);
            settings.SetActive(false);
            pause.SetActive(false);
            gameIsPaused = false;
            panel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void Setting()
    {
        settings.SetActive(true);
        pause.SetActive(false);
    }

    public void back()
    {
        settings.SetActive(false);
        pause.SetActive(true);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }


    public static bool getGameIsPaused()
    {
        return gameIsPaused;
    }
    public void effectSelect()
    {
        select.Play(0);
    }
    public void effectSelectBack()
    {
        selectBack.Play(0);
    }
}
