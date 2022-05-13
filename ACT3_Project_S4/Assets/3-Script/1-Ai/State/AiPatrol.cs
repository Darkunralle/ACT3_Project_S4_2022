using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrol : AiState
{
    public bool hold = false;
    public float holdTime = 10;

    //determine la distance minimal et maximal de la destination
    public float minDistance;
    public float maxDistance;

    //création de la variable de destination
    protected Vector3 randomPosition;

    public AiStateId GetId()
    {
        //retourne l'état actuel de l'agent
        return AiStateId.Patrol;
    }

    public void Enter(AiAgent agent)
    {
        //vitesse de l'agent dans l'etat actuel
        agent.navMeshAgent.speed = 2.5f;
    }
    public void Update(AiAgent agent)
    {  
        //l'agent n'a pas de destination ou l'agent est bloqué
        if (!agent.navMeshAgent.hasPath)
        {
            //récupère le game object possédant les worldBound
            WorldBound worldBounds = GameObject.FindObjectOfType<WorldBound>();
            Vector3 min = worldBounds.minBound.position;
            Vector3 max = worldBounds.maxBound.position;

            //génere un position aléatoire
            randomPosition = new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z)  
                );
            //met a jour la destination
            agent.navMeshAgent.destination = randomPosition;
        }

        //le sensor a detecter le joueur dans le sensor
        if (agent.sensor.IsInSight(agent.playerTransform.gameObject))
        {
            //vérifie si la liste possede un élément
            if (agent.sensor.Objects.Count > 0)
            {
                    agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
            }
        }        
    }

    public void Exit(AiAgent agent)
    {
    }
}
