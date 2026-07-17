using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeFromStartScene : MonoBehaviour
{
    [SerializeField] private float m_delay;

    [SerializeField] private IntRunTime m_level;

    public void ChangeScene(string scene)
    {
        m_level.SetValue(1);

        StartCoroutine(ChangeSceneDelay(scene));
    }

    private IEnumerator ChangeSceneDelay(string scene)
    {
        yield return new WaitForSeconds(m_delay);
        SceneManager.LoadScene(scene);
    }
}
