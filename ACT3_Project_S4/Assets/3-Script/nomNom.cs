using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nomNom : MonoBehaviour
{

    public GameObject parent;
    
    private PlayerMove m_player;
    public void OnTriggerEnter(Collider player)
    {
        Debug.Log("Joueur dans la zone");
        if (player.name == "Player")
        {
            m_player = player.GetComponent<PlayerMove>();
            if (m_player.attackPrey(20))
            {
                Debug.Log("Grailled");
                parent.SetActive(false);
            }
        }
    }
} 

