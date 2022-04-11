using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pause;

    public GameObject panel;

    public GameObject stamina;

    public GameObject settings;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsPaused)
            {
                Resume();
            }

            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.visible = false;
        stamina.SetActive(true);
        pause.SetActive(false);
        GameIsPaused = false;
        Time.timeScale = 1f;
        panel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        Cursor.visible = true;
        stamina.SetActive(false);
        pause.SetActive(true);
        GameIsPaused = true;
        Time.timeScale = 0f;
        panel.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
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

    public void backToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1f;
    }
}
