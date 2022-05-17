using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private AiAgent m_agent;

    private void OnTriggerStay(Collider foe)
    {
        if (foe.name == "Agent")
        {
            Debug.Log("Alerte");
            m_agent = foe.GetComponent<AiAgent>();
            m_agent.stateMachine.ChangeState(AiStateId.Sond);
            m_agent.navMeshAgent.SetDestination(transform.position);
        }
        else if (foe.name == "Ennemi")
        {
            Debug.Log("mob tuto");
        }
        
    }
}
