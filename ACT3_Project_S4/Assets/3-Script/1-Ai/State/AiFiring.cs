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

        if (agent.sensor.offset.sqrMagnitude <= 1500)
        {
            //agent.sensor.raycastTarget.transform.localPosition += Random.insideUnitSphere * agent.config.inacuracy;
            agent.sensor.offset += Random.insideUnitSphere * agent.config.inacuracy;
            Debug.Log(agent.config.currentTimeRecovery);
            agent.navMeshAgent.isStopped = true;
            //agent.config.currentTimeRecovery = 0.25f;
            
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
        if (Physics.Raycast(agent.sensor.cannon.transform.position, agent.transform.forward, out hit, agent.config.range))
        {
            Debug.Log("je touche " + hit.transform.name);
        }
    }
}
