using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject settings;
    public GameObject credits;

    public AudioClip mainTheme;
    public AudioSource select;
    public AudioSource selectBack;

    public void PlayCinematique ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        Debug.Log($"{Cursor.visible} & {Cursor.lockState}");
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

    public void effectSelect()
    {
        select.Play(0);
    }
    public void effectSelectBack()
    {
        selectBack.Play(0);
    }
}
