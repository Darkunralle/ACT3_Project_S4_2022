using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateId
{
    //determine les states possible
    ChasePlayer,
    Patrol,
    Sond,
    Firing,
    Death,
}

public interface AiState
{
    //création de l'interface utiliser dans les different state de l'agent
    AiStateId GetId();
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent);
}
