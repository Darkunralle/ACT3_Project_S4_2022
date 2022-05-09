using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSondDetection : AiState
{

    private float m_waiting = 15;
    private float m_timer = 0;
    public AiStateId GetId()
    {
        return AiStateId.Sond;
    }

    public void Enter(AiAgent agent)
    {
        m_timer = 0;
    }
    public void Update(AiAgent agent)
    {
        
        if (!agent.navMeshAgent.hasPath)
        {
            if (m_timer < m_waiting)
            {
                m_timer += Time.deltaTime;
            }
            else
            {
                agent.stateMachine.ChangeState(AiStateId.Patrol);
            }

            if (m_timer <= (m_waiting / 3))
            {
                agent.transform.RotateAround(agent.transform.position, Vector3.up, 50 * Time.deltaTime);
            }else if (m_timer < m_waiting)
            {
                agent.transform.RotateAround(agent.transform.position, Vector3.up, -50 * Time.deltaTime);
            }



        }
        else
        {
            m_timer = 0;
        }
    }

    public void Exit(AiAgent agent)
    {
        m_timer = 0;
    }
}
