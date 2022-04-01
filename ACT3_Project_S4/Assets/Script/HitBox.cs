using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public IaHealth health;

    public void OntriggerEnter(Collider other)
    {
        Debug.Log("avant j'etais un aventurier. Et puis j'ai pris une fleche dans le genou");
    }
}
