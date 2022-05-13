using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFiring : AiState
{
    public AiStateId GetId()
    {
        //retourne l'état actuel de l'agent
        return AiStateId.Firing;
    }
    public void Enter(AiAgent agent)
    {
    }
    public void Update(AiAgent agent)
    {
        //vérifie la distance entre l'agent et le player
        if (agent.sensor.offset.sqrMagnitude <= 1500)
        {
            //agent.sensor.raycastTarget.transform.localPosition += Random.insideUnitSphere * agent.config.inacuracy;
            agent.sensor.offset += Random.insideUnitSphere * agent.config.inacuracy;
            //Debug.Log(agent.config.currentTimeRecovery);
            //stop l'agent
            agent.navMeshAgent.isStopped = true;
            //agent.config.currentTimeRecovery = 0.25f;
            
            //cooldown entre chaque tire
            if (agent.config.currentTimeRecovery >= 0)
            {
                agent.config.currentTimeRecovery -= 1f * Time.deltaTime;
            }

            //le cooldown est à 0
            if (agent.config.currentTimeRecovery <= 0)
            {
                //la cooldown est réinitialisé 
                agent.config.currentTimeRecovery = agent.config.maxTimeRecovery;
                //l'agent tire
                Shoot(agent);
                Debug.Log("l'agent tir");
            }
        }

        if (agent.sensor.offset.sqrMagnitude >= 1900 || agent.sensor.Objects.Count <= 0)
        {
            //change le state actuel de l'agent
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
            //l'agent n'est plus arreter
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
