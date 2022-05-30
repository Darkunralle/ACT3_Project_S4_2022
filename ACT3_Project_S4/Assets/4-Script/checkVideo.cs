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

    void Start()
    {
        loadingStat = 0;
        button.SetActive(false);
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        sliderLoading.value = loadingStat / 67;
=======
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        sliderLoading.value = loadingStat / 135;
>>>>>>> Stashed changes
        Debug.Log(loadingStat);
        if(loadingStat < 135) {loadingStat += Time.deltaTime;}
        if(loadingStat > 67) { text.SetActive(true); if (Input.GetKeyDown("space")) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); } ; }
        if(loadingStat >= 135) { sliderLoading.gameObject.SetActive(false); button.SetActive(true); text.SetActive(false); }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
