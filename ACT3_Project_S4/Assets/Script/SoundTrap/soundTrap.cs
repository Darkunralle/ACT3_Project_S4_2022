using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundTrap : MonoBehaviour
{
    private void Awake()
    {
    
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Crack");
    }
}
