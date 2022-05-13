using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDeath : AiState
{
    public AiStateId GetId()
    {
        //retourne l'état actuel de l'agent
        return AiStateId.Death;
    }

    public void Enter(AiAgent agent)
    {
        
    }
    public void Update(AiAgent agent)
    {
    }

    public void Exit(AiAgent agent)
    {
    }
}
