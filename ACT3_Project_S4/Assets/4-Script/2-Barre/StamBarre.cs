using UnityEngine;
using UnityEngine.UI;

public class StamBarre : MonoBehaviour
{
    [SerializeField, Tooltip("Slider de la stambarre")]
    private Slider m_stamSlider;

    public void setMaxStam(int p_maxStam)
    {
        m_stamSlider.maxValue = p_maxStam;
        m_stamSlider.value = p_maxStam;
    }

    public void setStam(int p_stam)
    {
        m_stamSlider.value = p_stam;
    }
}
