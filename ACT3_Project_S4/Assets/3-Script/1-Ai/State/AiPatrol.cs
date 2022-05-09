using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrol : AiState
{
    public bool hold = false;
    public float holdTime = 10;

    public float minDistance;
    public float maxDistance;

    protected Vector3 randomPosition;

    public AiStateId GetId()
    {
        return AiStateId.Patrol;
    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.speed = 2.5f;
    }
    public void Update(AiAgent agent)
    {  
        if (!agent.navMeshAgent.hasPath)
        {
            WorldBound worldBounds = GameObject.FindObjectOfType<WorldBound>();
            Vector3 min = worldBounds.minBound.position;
            Vector3 max = worldBounds.maxBound.position;

            randomPosition = new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z)  
                );
            
                agent.navMeshAgent.destination = randomPosition;
        }

        if (agent.sensor.IsInSight(agent.playerTransform.gameObject))
        {
            if (agent.sensor.Objects.Count > 0)
            {
                    agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
            }
        }        
    }

    public void Exit(AiAgent agent)
    {
    }
}
