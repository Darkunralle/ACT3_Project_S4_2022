using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFiring : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Firing;
    }
    public void Enter(AiAgent agent)
    {
        agent.emmissive.SetColor("_EmissionColor", Color.red);
        agent.animator.Play("Tir",0,0.25f);
    }
    public void Update(AiAgent agent)
    {
        //si l'agent est a porter
        if (agent.sensor.offset.sqrMagnitude <= 1500)
        {
            //check si le joueur est dans la sphere
            agent.sensor.offset += Random.insideUnitSphere * agent.config.inacuracy;
            //Debug.Log(agent.config.currentTimeRecovery);

            //l'agent s'arrète
            agent.navMeshAgent.isStopped = true;
            
            //cooldown de chaque tir
            if (agent.config.currentTimeRecovery >= 0)
            {
                agent.config.currentTimeRecovery -= 1f;
            }

            if (agent.config.currentTimeRecovery <= 0)
            {
                agent.config.currentTimeRecovery = agent.config.maxTimeRecovery;
                //fonction de tier
                Shoot(agent);
            }
        }

        if (agent.sensor.Objects.Count == 0)
        {
            Debug.Log("check aifiring");
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
            agent.navMeshAgent.isStopped = false;
        }
    }

    public void Exit(AiAgent agent)
    {
    }

    void Shoot(AiAgent agent)
    {
        RaycastHit hit;
        //implementer les dégats -- adapter les damage fonction de la check sphere (playerInEngagmentRange et/ou playerInDeathRange)
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
