using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private void OnTriggerStay(Collider foe)
    {
        Debug.Log("Alerte");
    }

    private void OnTriggerEnter(Collider foe)
    {
        Debug.Log("Alerte");
    }
}
