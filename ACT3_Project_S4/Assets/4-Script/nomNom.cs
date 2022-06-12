using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nomNom : MonoBehaviour
{

    public GameObject parent;

    public AudioSource neck;
    
    private PlayerMove m_player;
    private void Start()
    {
        neck = GameObject.Find("neck").GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider player)
    {
        Debug.Log("fdd");
        parent.SetActive(false);

        if (player.name == "Player")
        {
            neck.Play(0);
            m_player = player.GetComponent<PlayerMove>();
            m_player.setStam(20);
        }
    }
} 

