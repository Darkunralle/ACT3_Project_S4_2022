using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class checkVideo : MonoBehaviour
{
    public Slider sliderLoading;
    public float loadingStat;
    public GameObject button;
    public GameObject text;

    AsyncOperation asyncOperation;

    void Start()
    {
        loadingStat = 0;
        button.SetActive(false);
        text.SetActive(false);
        asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!asyncOperation.isDone )
        {
            asyncOperation.allowSceneActivation = false;
        }
        
        sliderLoading.value = loadingStat / 67;
        Debug.Log(loadingStat);
        if(loadingStat < 135) {loadingStat += Time.deltaTime;}
        if(loadingStat > 10) { text.SetActive(true); if (Input.GetKeyDown("space")) { asyncOperation.allowSceneActivation = true; } ; }
        if(loadingStat >= 135) { sliderLoading.gameObject.SetActive(false); button.SetActive(true); text.SetActive(false); }
    }

    public void PlayGame()
    {
        asyncOperation.allowSceneActivation = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
