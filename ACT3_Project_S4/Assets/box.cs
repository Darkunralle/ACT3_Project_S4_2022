using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class box : MonoBehaviour
{
    public RawImage image;

    private bool fading = false;

    private void Update()
    {
        if (fading == true)
        {
            Debug.Log("grrver");
            image.CrossFadeAlpha(1, .25f, false);
        }

        if (fading == false)
        {
            image.CrossFadeAlpha(0, .25f, false);
        }
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        fading = true;
    }

    private void OnTriggerExit(Collider other)
    {
        fading = false;
    }
}
