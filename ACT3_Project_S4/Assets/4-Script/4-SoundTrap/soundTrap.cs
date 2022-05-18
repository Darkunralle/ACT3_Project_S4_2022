using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundTrap : MonoBehaviour
{
    [SerializeField, Tooltip("Radius de détection en mètre")]
    private float m_trapRadius = 15;

    private PlayerMove m_collider;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Crack");

        if (other.name == "Player")
        {
            m_collider = other.GetComponent<PlayerMove>();
            m_collider.sphereRadiusModify(m_trapRadius, 0.5f);
        }  
    }
}
