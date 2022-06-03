using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBlockBox : MonoBehaviour
{
    private bool m_firstLock = false;
    private void OnTriggerStay(Collider other)
    {
        if (!m_firstLock)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerMove>().setCamLock(true);
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!m_firstLock)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerMove>().setCamLock(false);
            }
            m_firstLock = true;
        }
        
    }
}
