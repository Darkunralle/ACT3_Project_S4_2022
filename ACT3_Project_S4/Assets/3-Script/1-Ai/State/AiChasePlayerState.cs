using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayerState : AiState
{
    //récupère la position du player
    public Transform playerTransform;
    float timer = 0f;
    float countDown = 5f;

    public AiStateId GetId()
    {
        //retourne l'état actuel de l'agent
        return AiStateId.ChasePlayer;
    }

    public void Enter(AiAgent agent)
    {
        //détermine la vitesse de l'agent dans le state actuel
        agent.navMeshAgent.speed = 4f;
    }
    public void Update(AiAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        //cooldown de poursuite
        timer -= Time.deltaTime;
        
        //change la destination de l'agent pour la position du player
        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }

        //si le cooldown est supperieur à 0
        if (timer < 0f)
        {
            Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;

            //vérifie si le player n'est pas trop loin
            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }
            //réinitialise le cooldown de poursuite
            timer = agent.config.maxTime;
        }

        //si le player est detecter par le sensor
        if (agent.sensor.IsInSight(agent.playerTransform.gameObject))
        {

            if (agent.sensor.Objects.Count <= 0)
            {
                countDown -= 1 * Time.deltaTime;
                if(countDown <= 0)
                {
                    //retourne en patrouile
                    agent.stateMachine.ChangeState(AiStateId.Patrol);
                }
            }
        }
        //si l'engmentZone est true
        if (agent.sensor.playerInEngagmentRange == true)
        {
            //change de state pour tirer
            agent.stateMachine.ChangeState(AiStateId.Firing);
        }
    }

    public void Exit(AiAgent agent)
    {
    }
}
