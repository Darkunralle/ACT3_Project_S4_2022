using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AiAgentConfig : ScriptableObject
{
    //configuration des élément basique de l'agent
    public float maxTime = 1f;
    public float maxDistance = 1f;
    public float maxViewDistance = 5.0f;

    [Header("Agent Weapon Spec")]
    public float damage = 10f;
    public float range = 100f;
    public float currentTimeRecovery;
    public float maxTimeRecovery = 1.25f;
    [Range(0f,1f)]
    public float inacuracy;

    public bool activeSwitch = false;
}
