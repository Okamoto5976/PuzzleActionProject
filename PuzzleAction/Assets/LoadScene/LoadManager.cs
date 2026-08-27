using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadManager : MonoBehaviour
{
    public static LoadManager m_instance;

    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private float m_fadeTime = 0.5f;

    [SerializeField] private GameObject m_panel;

    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string  sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        m_panel.SetActive(true);

        yield return FadeOut();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(3f);
                operation.allowSceneActivation = true;
            }
            yield return null;
        }

        yield return FadeIn();

        m_panel.SetActive(false);

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

        while(time < m_fadeTime)
        {
            time += Time.unscaledDeltaTime;

            float t = time / m_fadeTime;
            m_canvasGroup.alpha = Mathf.Lerp(startAlpha, amount, t);

            yield return null;
        }

        m_canvasGroup.alpha = amount;
    }
}
