using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayerState : AiState
{
    public Transform playerTransform;
    float timer = 0f;
    float countDown;

    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }

    public void Enter(AiAgent agent)
    {
        countDown = 10f;
        agent.animator.Play("Marche", 0, 0.25f);
    }
    public void Update(AiAgent agent)
    {
        countDown -= Time.deltaTime;
        //Debug.Log(timer);
        if (!agent.enabled)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }

        if (timer < 0f)
        {
            Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;

            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }

            timer = agent.config.maxTime;
        }

        if (countDown <= 0) { agent.stateMachine.ChangeState(AiStateId.Patrol);}

    
        //passage en etat de tire
        if (agent.sensor.playerInEngagmentRange == true && agent.sensor.Objects.Count > 0)
            {
                agent.stateMachine.ChangeState(AiStateId.Firing);
            }
    }

        

    public void Exit(AiAgent agent)
    {
    }
}
