using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckDestination: ActionNode
{
    /// <summary>
    /// 
    /// Cr�ation d'une BoxCollider
    /// V�rification de la presence d'une destination d'un autre agent.
    /// 
    /// Si oui : g�n�ration d'une nouvelle destination.
    /// 
    /// Si non : Success.
    /// 
    /// </summary>
    protected SphereCollider destinationCheck;
    protected override void OnStart() {
        context.agent.destination = blackboard.moveToPosition;
        destinationCheck = blackboard.Sc;

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        destinationCheck.center = blackboard.moveToPosition;
        return State.Success;
    }
}
