using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IaHealth : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        throw new NotImplementedException();
    }
}
