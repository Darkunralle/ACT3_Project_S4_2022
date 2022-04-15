using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nomNom : MonoBehaviour
{

    public GameObject parent;
    
    private PlayerMove m_player;
    public void OnTriggerEnter(Collider player)
    {
        Debug.Log("fdd");
        parent.SetActive(false);

        if (player.name == "Player")
        {
            m_player = player.GetComponent<PlayerMove>();
            m_player.setStam(20);
        }
    }
} 

