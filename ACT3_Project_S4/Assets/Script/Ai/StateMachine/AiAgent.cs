using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;

    public AiStateId initialState;

    public NavMeshAgent navMeshAgent;

    public AiAgentConfig config;

    public Transform playerTransform;

    public AiSensor sensor;


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        stateMachine = new AiStateMachine(this);

        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiPatrol());
        stateMachine.RegisterState(new AiWarned());
        stateMachine.RegisterState(new AiDeath());

        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(stateMachine.currentState);
        stateMachine.Update();
    }
}
