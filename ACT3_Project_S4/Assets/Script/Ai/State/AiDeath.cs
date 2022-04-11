using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDeath : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Death;
    }

    public void Enter(AiAgent agent)
    {
        
    }
    public void Update(AiAgent agent)
    {
        throw new System.NotImplementedException();
    }

    public void Exit(AiAgent agent)
    {
        throw new System.NotImplementedException();
    }
}
