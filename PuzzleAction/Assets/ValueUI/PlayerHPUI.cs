using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [Header("Reference")]
    //å„Ç≈ê›íËÇ∑ÇÈ
    [SerializeField] private PlayerHP m_playerHP; 

    [Header("PlayerHP UI")]
   // [SerializeField] private Slider m_HPSlider;
    [SerializeField] private Image m_FrontHPImage;
    [SerializeField] private Image m_BackHPImage;
    [SerializeField] private TMP_Text m_HPText;

    [Header("Slider Speed")]
    [SerializeField] private float m_BackSpeed = 0.5f;

    private void Update()
    { 
        if (m_playerHP == null)
            return;

        int maxHP = m_playerHP.MaxHP;
        int currentHP = m_playerHP.CurrentHP;

        float hoRate = (float)currentHP / maxHP;

        if(m_FrontHPImage != null)
        {
            m_FrontHPImage.fillAmount = hoRate;
        }

        if(m_BackHPImage != null)
        {
            m_BackHPImage.fillAmount = Mathf.MoveTowards(
                m_BackHPImage.fillAmount,
                hoRate,
                m_BackSpeed * Time.deltaTime
                );
        }

        if (m_HPText != null)
        {
            m_HPText.text = $"{m_playerHP.CurrentHP} / {m_playerHP.MaxHP}";
        }
    }
}
