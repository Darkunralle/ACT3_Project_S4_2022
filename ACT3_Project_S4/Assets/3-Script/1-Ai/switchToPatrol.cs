using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchToPatrol : AiAgent
{


    private void OnTriggerEnter(Collider player)
    {
        Debug.Log("switch to On");
        //activated();
        
    }
    private void activated(AiAgent agent) 
    {
        agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
    }
}
