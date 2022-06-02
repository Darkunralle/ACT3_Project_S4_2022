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

    // Pour les cas ou l'audio se trouve dans des prefab
    private static List<AudioSource> m_staticBruitage = new List<AudioSource>();
    private List<AudioSource> m_staticMusique = new List<AudioSource>();

    // Potentiel problème de list qui se surcharge mais jugé négligeable
    public static void addMusicToList(AudioSource p_toAdd)
    {
        m_staticBruitage.Add(p_toAdd);
    }

    public static void addSonoreEffectToList(AudioSource p_toAdd)
    {
        m_staticBruitage.Add(p_toAdd);
    }

    public static void forceUpdateMusic(AudioSource p_toChange)
    {
        if (PlayerPrefs.HasKey("musique"))
        {
            p_toChange.volume = PlayerPrefs.GetFloat("musique");
        }
        else
        {
            p_toChange.volume = 0.5f;
        }
        
    }

    public static void forceUpdateSonoreEffect(AudioSource p_toChange)
    {
        if (PlayerPrefs.HasKey("sound"))
        {
            p_toChange.volume = PlayerPrefs.GetFloat("sound");
        }
        else
        {
            p_toChange.volume = 0.5f;
        }
    }


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
                if (m_musique.Count != 0)
                {
                    foreach (AudioSource row in m_musique)
                    {
                        row.volume = m_musiqueValue;
                    }
                }

                if (m_staticMusique.Count != 0)
                {
                    foreach (AudioSource row in m_staticMusique)
                    {
                        row.volume = m_musiqueValue;
                    }
                }

            }   
        }
        if (PlayerPrefs.HasKey("sound"))
        {
            if (PlayerPrefs.GetFloat("sound") != m_bruitageValue)
            {
                m_bruitageValue = PlayerPrefs.GetFloat("sound");
                if (m_bruitage.Count != 0)
                {
                    foreach (AudioSource row in m_bruitage)
                    {
                        row.volume = m_bruitageValue;
                    }
                }
                if (m_staticBruitage.Count != 0)
                {
                    foreach (AudioSource row in m_staticBruitage)
                    {
                        row.volume = m_bruitageValue;
                    }
                }
            }
        }
    }
}
