using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeFromStartScene : MonoBehaviour
{
    [SerializeField] private float m_delay;

    public void ChangeScene(string scene)
    {
        StartCoroutine(ChangeSceneDelay(scene));
    }

    private IEnumerator ChangeSceneDelay(string scene)
    {
        yield return new WaitForSeconds(m_delay);
        SceneManager.LoadScene(scene);
    }
}
