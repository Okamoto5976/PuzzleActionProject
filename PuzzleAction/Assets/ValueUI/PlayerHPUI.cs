using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private PlayerHP m_playerHP; 

    [Header("PlayerHP UI")]
    [SerializeField] private Slider m_HPSlider;
    [SerializeField] private TMP_Text m_HPText;

    public void Update()
    { 
        if (m_playerHP == null)
            return;

        m_HPSlider.maxValue = m_playerHP.MaxHP;
        m_HPSlider.value = m_playerHP.CurrentHP;

        if (m_HPText != null)
        {
            m_HPText.text = $"{m_playerHP.CurrentHP} / {m_playerHP.MaxHP}";
        }
    }
}
