using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTutoriel : AiState
{
    private bool sound = false;
    public AiStateId GetId()
    {
        return AiStateId.Tutoriel;
    }
    public void Enter(AiAgent agent)
    {
        sound = false;
        agent.animator.Play("Idle", 0, 0.25f);
    }
    public void Update(AiAgent agent)
    {
        if (agent.sensor.IsInSight(agent.playerTransform.gameObject))
        {
            agent.animator.Play("Tir", 0, 0.25f);
            if (agent.sensor.Objects.Count > 0)
            {
                if (!sound) { agent.detection.Play(0); sound = true; }
                agent.sensor.offset += Random.insideUnitSphere * agent.config.inacuracy;
                    //Debug.Log(agent.config.currentTimeRecovery);  
                agent.navMeshAgent.isStopped = true;
                    
                if (agent.config.currentTimeRecovery >= 0)                
                {
                    agent.config.currentTimeRecovery -= 1f;
                }
                    
                if (agent.config.currentTimeRecovery <= 0)
                {
                    agent.config.currentTimeRecovery = agent.config.maxTimeRecovery;
                    Shoot(agent);
                }
            }
        }
        else
            agent.stateMachine.ChangeState(AiStateId.Tutoriel);
    }
    public void Exit(AiAgent agent)
    {
    }

    void Shoot(AiAgent agent)
    {
        Debug.Log("Poke");
        RaycastHit hit;
        if (Physics.Raycast(agent.sensor.cannon.transform.position, agent.transform.forward, out hit, agent.config.range) && !PauseButton.m_timeToStop)
        {
            
            agent.arFiring.Play(0);
            agent.arImpact.Play(0);
            agent.muzzleFlash.Play();

            if (!agent.sensor.playerInDeathRange && agent.sensor.playerInEngagmentRange)
            {
                PlayerMove.beHit(false);
            }

            if (agent.sensor.playerInDeathRange && agent.sensor.playerInEngagmentRange)
            {
                PlayerMove.beHit(true);
            }
        }
    }
}
