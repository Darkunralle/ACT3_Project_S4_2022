using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckDestination: ActionNode
{
    protected override void OnStart() {
        context.agent.destination = blackboard.moveToPosition;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
