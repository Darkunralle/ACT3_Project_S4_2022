using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class Loco : MonoBehaviour
{
    NavMeshAgent agent;

    public Transform playerTransform;

    public float maxTime = 1f;
    public float maxDistance = 1f;

    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
