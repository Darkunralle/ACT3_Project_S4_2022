using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IaSpawner : MonoBehaviour
{

    [SerializeField, Tooltip("Nombre d'ia qui doit mourir avant de en faire spawn d'autre < au nb de spawn")]
    private int m_numberOfDeath = 2;

    [SerializeField, Tooltip("Nombre d'ia qui spawn d'un coup")]
    private int m_spawningNumber = 3;

    [SerializeField, Tooltip("Prefab de l'agent")]
    private GameObject m_agentPrefab;

    // Compteur du nombre d'agent mort
    private int m_deathCompter = 0;

    [SerializeField, Tooltip("Liste de spawn")]
    private List<Transform> m_listeSpawnT;

    // Same mais les passe en vecteur3
    private List<Vector3> m_listeSpawnV;


    private void OnEnable()
    {
        PlayerMove.m_spawnCp += setDeathCompter;
    }

    private void OnDisable()
    {
        PlayerMove.m_spawnCp -= setDeathCompter;
    }
    private void Start()
    {

        if (m_listeSpawnT == null)
        {
            Debug.Log("Aucune position de spawn est renseignée, initialisatio nde valeur par défaut");

            m_listeSpawnV.Add(new Vector3(-115, 0, 60));
            m_listeSpawnV.Add(new Vector3(0, 5, 19));
            m_listeSpawnV.Add(new Vector3(8, 1, -4));
        }
        else
        {
            for (int i = 0; i < m_listeSpawnT.Count; i++)
            {
                m_listeSpawnV.Add(m_listeSpawnT[i].position);
            }
            
        }
    }

    void Update()
    {
        if (m_deathCompter >= m_numberOfDeath)
        {
            m_deathCompter -= m_numberOfDeath;
            // spawn un nombre d'agent selon le nombre défini
            for (int i = 0; i < m_spawningNumber; i++)
            {
                Debug.Log("SPAWN");
                Instantiate(m_agentPrefab, getAPosForAgent(), Quaternion.identity);
            }
            
        }
    }

    // Donne une position aléatoire pour le spawn de l'ia
    private Vector3 getAPosForAgent()
    {
        Vector3 agentPos = m_listeSpawnV[Random.Range(0, m_listeSpawnV.Count)];
        return agentPos;
    }

    void setDeathCompter()
    {
        m_deathCompter++;
        Debug.Log("Kill");
    }
}
