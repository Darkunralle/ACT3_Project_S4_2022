using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ik : MonoBehaviour
{
    public Transform target;
    public Transform aim;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = target.transform.position;
        AimAtTarget(targetPosition);
    }

    private void AimAtTarget(Vector3 targetPosition)
    {
        Vector3 aimDirection = aim.forward;
        Vector3 targetDirection = targetPosition - aim.position;
        Quaternion aimToward = Quaternion.FromToRotation(aimDirection, targetDirection);
    }
}
