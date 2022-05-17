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

        if (agent.sensor.offset.sqrMagnitude >= 1900 || agent.sensor.Objects.Count <= 0)
        {
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
        if (Physics.Raycast(agent.sensor.cannon.transform.position, agent.transform.forward, out hit, agent.config.range))
        {
            //Debug.Log("je touche " + hit.transform.name);
        }
    }
}
