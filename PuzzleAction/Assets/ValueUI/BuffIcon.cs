using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    private bool m_isActive;

    public bool IsActive => m_isActive;

    private TemporaryBuffInstance m_instance;
    private Sprite m_sprite;

    [SerializeField] private Image m_image;
    [SerializeField] private TMP_Text m_text;

    public void SetData(TemporaryBuffInstance instance, Sprite sprite)
    {
        m_isActive = true;
        gameObject.SetActive(true);
        m_instance = instance;
        m_sprite = sprite;

        m_image.sprite = m_sprite;
    }

    private void Update()
    {
        if (!m_isActive) return;
        if (m_instance == null) return;

        m_text.text =
            Mathf.CeilToInt(m_instance.m_duration).ToString();

        if(m_instance.m_duration <= 0f)
        {
            m_isActive = false;
            gameObject.SetActive(false);
            m_instance = null;
        }
    }
}
