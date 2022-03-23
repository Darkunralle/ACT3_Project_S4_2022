using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class Rotate : ActionNode
{
    private GameObject agent;
    private Transform transform;
    public float duration = 1;
    float startTime;

    public bool updateRotation = false;

    protected override void OnStart() {
        startTime = Time.time;
        agent = blackboard.Agent;
        context.agent.updateRotation = updateRotation;

        transform = agent.GetComponent<Transform>();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        transform.Rotate(0,+90f,0);

        return State.Running;
    }
}
