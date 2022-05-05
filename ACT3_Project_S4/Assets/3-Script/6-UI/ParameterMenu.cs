using UnityEngine;
using UnityEngine.UI;

public class ParameterMenu : MonoBehaviour
{
    [SerializeField, Tooltip("Slider g�rant la sensibilit�")]
    private Slider m_sensitiveSlider;

    [SerializeField, Tooltip("Slider g�rant le son")]
    private Slider m_soundSlider;

    [SerializeField, Tooltip("Slider g�rant la musique")]
    private Slider m_musicSlider;

    private float m_sensiSave = 0.5f;
    private float m_soundSave = 0.5f;
    private float m_musicSave = 0.5f;


    private void Start()
    {
        if (PlayerPrefs.HasKey("sensitive"))
        {
            m_sensitiveSlider.value = PlayerPrefs.GetFloat("sensitive");
            m_sensiSave = m_sensitiveSlider.value;
        }
        else m_sensitiveSlider.value = m_sensiSave;

        if (PlayerPrefs.HasKey("musique"))
        {
            m_musicSlider.value = PlayerPrefs.GetFloat("musique");
            m_musicSave = m_musicSlider.value;
        }
        else m_musicSlider.value = m_musicSave;

        if (PlayerPrefs.HasKey("sound"))
        {
            m_soundSlider.value = PlayerPrefs.GetFloat("sound");
            m_soundSave = m_soundSlider.value;
        }    
        else m_soundSlider.value = m_soundSave;

        m_sensitiveSlider.minValue = 0.1f;
        m_sensitiveSlider.maxValue = 1f;
    }
    public void save()
    {
        PlayerPrefs.SetFloat("sensitive", m_sensitiveSlider.value);
        PlayerPrefs.SetFloat("musique", m_musicSlider.value);
        PlayerPrefs.SetFloat("sound", m_soundSlider.value);

        m_sensiSave = m_sensitiveSlider.value;
        m_soundSave = m_soundSlider.value;
        m_musicSave = m_musicSlider.value;

    }

    public void undo()
    {
        m_sensitiveSlider.value = m_sensiSave;
        m_soundSlider.value = m_soundSave;
        m_musicSlider.value = m_musicSave;
    }

    private void Update()
    {
        if (!m_sensitiveSlider.isActiveAndEnabled) m_sensitiveSlider.value = m_sensiSave;
        if (!m_soundSlider.isActiveAndEnabled) m_soundSlider.value = m_soundSave;
        if (!m_musicSlider.isActiveAndEnabled) m_musicSlider.value = m_musicSave;
    }

}
