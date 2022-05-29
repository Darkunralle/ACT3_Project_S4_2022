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

    void Start()
    {
        loadingStat = 0;
        button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        sliderLoading.value = loadingStat / 135;
        Debug.Log(loadingStat);
        if(loadingStat < 135) {loadingStat += Time.deltaTime;}
        if(loadingStat >= 135) { sliderLoading.gameObject.SetActive(false); button.SetActive(true); }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
