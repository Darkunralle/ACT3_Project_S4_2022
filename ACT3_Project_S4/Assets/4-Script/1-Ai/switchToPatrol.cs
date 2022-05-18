using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchToPatrol : MonoBehaviour
{
    public bool activeSwitch = false;

    private void OnTriggerEnter(Collider player)
    {
        Debug.Log("switch to On");
        activeSwitch = true;
    }
}
