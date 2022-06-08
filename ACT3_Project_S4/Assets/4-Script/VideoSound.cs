using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoSound : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer m_video;

    void Start()
    {
        if (PlayerPrefs.HasKey("musique"))
        {
            m_video.SetDirectAudioVolume(0, PlayerPrefs.GetFloat("musique"));
        }
        else
        {
            m_video.SetDirectAudioVolume(0, 0.5f);
        }
        
    }

}
