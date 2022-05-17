using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiWaitingForSwitch : AiState
{
    public AiStateId GetId()
    {
<<<<<<<< HEAD:ACT3_Project_S4/Assets/4-Script/1-Ai/State/AiWaitingForSwitch.cs
        return AiStateId.WaitingForSwitch;
========
        //retourne l'état actuel de l'agent
        return AiStateId.Death;
>>>>>>>> Working:ACT3_Project_S4/Assets/4-Script/1-Ai/State/AiDeath.cs
    }
    public void Enter(AiAgent agent)
    {
    }
    public void Update(AiAgent agent)
    {
<<<<<<<< HEAD:ACT3_Project_S4/Assets/4-Script/1-Ai/State/AiWaitingForSwitch.cs
        if (agent.switchstate.activeSwitch)
        {
            agent.stateMachine.ChangeState(AiStateId.Patrol);
        }
        else { };
========
>>>>>>>> Working:ACT3_Project_S4/Assets/4-Script/1-Ai/State/AiDeath.cs
    }

    public void Exit(AiAgent agent)
    {
    }
}
