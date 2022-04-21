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
        agent.navMeshAgent.Stop();
    }
    public void Update(AiAgent agent)
    {

        if (agent.sensor.offset.sqrMagnitude >= 1900)
        {
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }
    }

    public void Exit(AiAgent agent)
    {
    }
}
