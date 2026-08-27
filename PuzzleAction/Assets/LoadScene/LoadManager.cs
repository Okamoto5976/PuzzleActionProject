using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadManager : MonoBehaviour
{
    public static LoadManager m_instance;

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
    }
}
