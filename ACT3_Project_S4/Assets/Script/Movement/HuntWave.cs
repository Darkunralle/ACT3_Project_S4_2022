using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntWave : MonoBehaviour
{
    public Rigidbody self;
    public bool activeHunt = false;
    [SerializeField] private float speed = 100f;

    void Update()
    {
        if (activeHunt) self.velocity = new Vector3(0, 0,speed * Time.deltaTime);
    }
}
