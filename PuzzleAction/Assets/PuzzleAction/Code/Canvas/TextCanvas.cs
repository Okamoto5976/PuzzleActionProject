using UnityEngine;
using System.Collections;
using TMPro;

public class TextCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private float m_fadeTime = 0.5f;

    [SerializeField] private TMP_Text m_levelText;
    [SerializeField] private string m_text;

    private void Start()
    {
        m_levelText.text = GameManager.Instance.Level.ToString() + m_text;
    }


    public IEnumerator FadeOut()
    {
        yield return Fade(1f);
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(0f);

    }
    private IEnumerator Fade(float amount)
    {
        float startAlpha = m_canvasGroup.alpha;
        float time = 0f;

        while (time < m_fadeTime)
        {
            time += Time.unscaledDeltaTime;

            float t = time / m_fadeTime;
            m_canvasGroup.alpha = Mathf.Lerp(startAlpha, amount, t);

            yield return null;
        }

        m_canvasGroup.alpha = amount;
    }
}
