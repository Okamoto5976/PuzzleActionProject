using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LoadManager : MonoBehaviour
{
    public static LoadManager m_instance;

    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private float m_fadeTime = 0.5f;

    [SerializeField] private GameObject m_panel;

    [SerializeField] private TMP_Text m_tipsText;
    private int m_lastTip = -1;
    [TextArea(2, 5)]
    [SerializeField]    private string[] m_tips =
        {
        "炎、毒、ガス のバフダメージは HP１残ります",
        "アイテムの[ダイナマイト]　[けおどし爆弾]は　使用者も　ダメージを受けます",
        "特定のアイテムを揃えて持っておくといいことが...？",
        "マップピース設置は スタート地点に 危険を置かないよう気を付けましょう",
        "５階層ごとにボスがおり、倒さないとゴールはできません",
        };

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

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        if (m_tips.Length > 0)
        {
            int index;

            do
            {
                index = Random.Range(0, m_tips.Length);
            }
            while (m_tips.Length > 1 && index == m_lastTip);

            m_lastTip = index;
            m_tipsText.text = m_tips[index];
        }
        m_panel.SetActive(true);

        yield return FadeOut();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                //yield return new WaitForSeconds(3f);
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
