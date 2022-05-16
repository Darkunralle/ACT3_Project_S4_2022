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
        
    }
    public void Update(AiAgent agent)
    {
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
