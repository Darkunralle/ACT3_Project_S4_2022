using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class checkVideoEnd : MonoBehaviour
{
    public Slider sliderLoading;
    public float loadingStat;

    void Start()
    {
        loadingStat = 0;
    }

    // Update is called once per frame
    void Update()
    {        
        sliderLoading.value = loadingStat / 60;
        Debug.Log(loadingStat);
        if(loadingStat < 61) {loadingStat += Time.deltaTime;}
        if (loadingStat >= 61) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3); };
    }

    public void PlayGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
