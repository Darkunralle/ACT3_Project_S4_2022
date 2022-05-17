using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject settings;
    public GameObject credits;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Setting()
    {
        settings.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void Credits()
    {
        credits.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void back()
    {
        mainMenu.SetActive(true);
        settings.SetActive(false);
        credits.SetActive(false);
    }

    public void quit()
    {
        Application.Quit();
    }
}
