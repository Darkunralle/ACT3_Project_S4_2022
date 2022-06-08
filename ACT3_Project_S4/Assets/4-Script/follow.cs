using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    public Transform player;
    public Transform snow;

    // Update is called once per frame
    void Update()
    {
        snow.transform.position = player.transform.position;
    }
}
