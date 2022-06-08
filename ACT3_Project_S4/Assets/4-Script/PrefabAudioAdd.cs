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

    void Start()
    {
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
        }   
    }
}
