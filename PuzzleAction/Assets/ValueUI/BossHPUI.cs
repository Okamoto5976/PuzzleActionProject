using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    [Header("Reference")]
    //Œã‚ÅÝ’è‚·‚é
    [SerializeField] private Middleman_BossEnemy _middlemanBossEnemy;
    private float BossHP => _middlemanBossEnemy.BossHP;
    private float BossMaxHP => _middlemanBossEnemy.BossMaxHP;

    [Header("PlayerHP UI")]
   // [SerializeField] private Slider m_HPSlider;
    [SerializeField] private Image m_FrontHPImage;
    [SerializeField] private Image m_BackHPImage;
    //[SerializeField] private TMP_Text m_HPText;

    [Header("Slider Speed")]
    [SerializeField] private float m_BackSpeed = 0.5f;

    private bool _currentUIState = false;

    private void Update()
    {
        if (_middlemanBossEnemy == null) return;
        if (!_middlemanBossEnemy.IsBossActive)
        {
            if (!_currentUIState) return;
            SetUIState(false);
        }
        if (!_currentUIState)
        {
            SetUIState(true);
        }

        float hoRate = (float)BossHP / BossMaxHP;

        //Debug.Log(hoRate);

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

        //if (m_HPText != null)
        //{
        //    m_HPText.text = $"{m_playerHP.CurrentHP} / {m_playerHP.MaxHP}";
        //}
    }

    private void SetUIState(bool state)
    {
        _currentUIState = state;
        Debug.LogWarning($"set boss ui state to {state}");
        foreach (RectTransform child in transform)
        {
            child.gameObject.SetActive(state);
        }
    }
}
