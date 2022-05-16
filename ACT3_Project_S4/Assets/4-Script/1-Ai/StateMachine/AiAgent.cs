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
        //récupere le navmesh générer
        navMeshAgent = GetComponent<NavMeshAgent>();

        stateMachine = new AiStateMachine(this);

        //enregistre les states disponible
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiPatrol());
        stateMachine.RegisterState(new AiSondDetection());
        stateMachine.RegisterState(new AiFiring());
        stateMachine.RegisterState(new AiDeath());

        stateMachine.ChangeState(initialState);
    }

    // Update le state actuel de l'agent
    void Update()
    {
        //Debug.Log(stateMachine.currentState);
        stateMachine.Update();
    }
}
