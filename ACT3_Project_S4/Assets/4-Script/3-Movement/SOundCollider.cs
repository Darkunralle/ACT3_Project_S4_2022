using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOundCollider : MonoBehaviour
{
    private AiAgent agent;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            Debug.Log("chase");
            agent = other.GetComponent<AiAgent>();
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }
    }
}
