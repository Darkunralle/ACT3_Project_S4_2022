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

    public switchToPatrol switchstate;

    public AudioSource detection;

    public AudioSource arFiring;

    public AudioSource arImpact;

    public AudioSource walk;

    public ParticleSystem muzzleFlash;

    // Start is called before the first frame update
    void Start()
    {
        GlobalSoundController.addSonoreEffectToList(detection);
        GlobalSoundController.forceUpdateSonoreEffect(detection);
        GlobalSoundController.addSonoreEffectToList(arFiring);
        GlobalSoundController.forceUpdateSonoreEffect(arFiring);
        GlobalSoundController.addSonoreEffectToList(arImpact);
        GlobalSoundController.forceUpdateSonoreEffect(arImpact);
        GlobalSoundController.addSonoreEffectToList(walk);
        GlobalSoundController.forceUpdateSonoreEffect(walk);

        playerTransform = GameObject.Find("Player").transform;

        //detection = FindObjectOfType<AudioSource>(GameObject.Find("Detected"));

        switchstate = FindObjectOfType<switchToPatrol>(switchstate);

        navMeshAgent = GetComponent<NavMeshAgent>();

        stateMachine = new AiStateMachine(this);

        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiPatrol());
        stateMachine.RegisterState(new AiFiring());
        stateMachine.RegisterState(new AiWaiting());
        stateMachine.RegisterState(new AiWaitingForSwitch());
        stateMachine.RegisterState(new AiTutoriel());

        stateMachine.ChangeState(initialState);
    }


    public void spawnChasing()
    {
        stateMachine.ChangeState(AiStateId.ChasePlayer);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(stateMachine.currentState);
        stateMachine.Update();
    }
}
