using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("avant j'etais un aventurier. Et puis j'ai pris une fleche dans le genou");
        PlayerMove.attackPrey();
    }
}
