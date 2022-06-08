using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabAudioAdd : MonoBehaviour
{
    // Script a utiliser sur les prefab ayant juste une audioSource sans code qui apelle le son

    [SerializeField, Tooltip("Liste de ou des effets sonore lié a l'objet")]
    private List<AudioSource> m_audioListEffect;

    [SerializeField, Tooltip("Liste de ou des musiques de lié a l'objet")]
    private List<AudioSource> m_audioListMusic;

    private float m_musiqueValue;
    private float m_bruitageValue;

    void Start()
    {
        /*
        if (m_audioListEffect.Count != 0)
        {
            foreach (AudioSource row in m_audioListEffect)
            {
                GlobalSoundController.addSonoreEffectToList(row);
                GlobalSoundController.forceUpdateSonoreEffect(row);
            }
        }

            if (m_audioListMusic.Count !=0)
        {
            foreach (AudioSource row in m_audioListMusic)
            {
                GlobalSoundController.addMusicToList(row);
                GlobalSoundController.forceUpdateMusic(row);
            }
        }   */

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
                if (m_audioListMusic.Count != 0)
                {
                    foreach (AudioSource row in m_audioListMusic)
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
                if (m_audioListEffect.Count != 0)
                {
                    foreach (AudioSource row in m_audioListEffect)
                    {
                        row.volume = m_bruitageValue;
                    }
                }
            }
        }
    }
}
