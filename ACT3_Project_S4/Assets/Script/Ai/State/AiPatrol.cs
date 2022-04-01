using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrol : AiState
{
    public Vector2 min = Vector2.one * -10;
    public Vector2 max = Vector2.one * 10;

    Vector3 moveToPosition;

    public float minDistance;
    public float maxDistance;

    public AiStateId GetId()
    {
        return AiStateId.Patrol;
    }

    public void Enter(AiAgent agent)
    {
        moveToPosition.x = Random.Range(min.x, max.x);
        moveToPosition.y = Random.Range(min.y, max.y);
    }
    public void Update(AiAgent agent)
    {
        Debug.Log(moveToPosition);
    }

    public void Exit(AiAgent agent)
    {
    }
}
