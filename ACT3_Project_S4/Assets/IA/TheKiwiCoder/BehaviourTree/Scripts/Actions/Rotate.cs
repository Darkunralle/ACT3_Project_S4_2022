using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class Rotate : ActionNode
{
    private GameObject agent;
    public float duration = 1;
    float startTime;

    protected override void OnStart() {
        startTime = Time.time;
        agent = blackboard.Agent;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        agent.transform.Rotate(Vector3.up, 50f * Time.deltaTime);
        if (Time.time - startTime > duration)
        {
            return State.Success;
        }
        return State.Running;
    }
}
