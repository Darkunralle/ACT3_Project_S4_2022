using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IaHealth : MonoBehaviour
{

    public float maxHealth;
    
    [HideInInspector]
    public float currentHealth;

    AiAgent agent;
    
    void Start()
    {
        agent = GetComponent<AiAgent>();
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
        AiDeath deathState = agent.stateMachine.GetState(AiStateId.Death) as AiDeath;
        agent.stateMachine.ChangeState(AiStateId.Death);
    }
}
