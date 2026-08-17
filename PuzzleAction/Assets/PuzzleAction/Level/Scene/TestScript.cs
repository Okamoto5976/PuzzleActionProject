using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    [SerializeField] private TestDB m_DB;

    public void OnClick()
    {
        m_DB.AddValue(1);

        SceneManager.LoadScene("SceneB");
    }
}
