using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiWaiting : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Waiting;
    }

    public void Enter(AiAgent agent)
    {
        agent.animator.Play("Idle", 0, 0.25f);
    }
    public void Update(AiAgent agent)
    {
        if (agent.sensor.Objects.Count > 0)
        {
            agent.detection.Play(0);
            Debug.Log("njkvernyu");
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
            
            if (agent.sensor.playerInEngagmentRange == true)
            {
                agent.stateMachine.ChangeState(AiStateId.Firing);
            }
        }
        
    }

    public void Exit(AiAgent agent)
    {
    }
}
