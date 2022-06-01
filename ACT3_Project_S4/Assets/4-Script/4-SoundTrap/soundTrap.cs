using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundTrap : MonoBehaviour
{
    [SerializeField, Tooltip("Radius de d�tection en m�tre")]
    private float m_trapRadius = 15;

    private PlayerMove m_collider;

    public AudioSource crick;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Crack");
        crick.Play(0);

        if (other.name == "Player")
        {
            m_collider = other.GetComponent<PlayerMove>();
            m_collider.sphereRadiusModify(m_trapRadius, 0.5f);
        }  
    }
}
