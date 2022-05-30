using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSoundController : MonoBehaviour
{
    [SerializeField, Tooltip("Liste des musiques")]
    private List<AudioSource> m_musique;

    private float m_musiqueValue;

    [SerializeField, Tooltip("Liste des effets sonores")]
    private List<AudioSource> m_bruitage;

    private float m_bruitageValue;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musique"))
        {
            m_musiqueValue = PlayerPrefs.GetFloat("musique");
        }
        if (PlayerPrefs.HasKey("sound"))
        {
            m_bruitageValue = PlayerPrefs.GetFloat("sound");
        }
    }
    private void Update()
    {
        if (PlayerPrefs.HasKey("musique"))
        {
            // Empèche de reparcourir  la liste a chaque update
            if (PlayerPrefs.GetFloat("musique") != m_musiqueValue)
            {
                m_musiqueValue = PlayerPrefs.GetFloat("musique");

                foreach (AudioSource row in m_musique)
                {
                    row.volume = m_musiqueValue;
                }
            }   
        }
        if (PlayerPrefs.HasKey("sound"))
        {
            if (PlayerPrefs.GetFloat("sound") != m_bruitageValue)
            {
                m_bruitageValue = PlayerPrefs.GetFloat("sound");

                foreach (AudioSource row in m_bruitage)
                {
                    row.volume = m_bruitageValue;
                }
            }
        }
    }
}
