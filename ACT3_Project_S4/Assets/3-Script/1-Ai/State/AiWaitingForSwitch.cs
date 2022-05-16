using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiWaitingForSwitch : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.WaitingForSwitch;
    }
    public void Enter(AiAgent agent)
    {
    }
    public void Update(AiAgent agent)
    {
        if (agent.config.timeToGo == true)
        {
            agent.stateMachine.ChangeState(AiStateId.Patrol);
        }
        else { };
    }

    public void Exit(AiAgent agent)
    {
    }
}
