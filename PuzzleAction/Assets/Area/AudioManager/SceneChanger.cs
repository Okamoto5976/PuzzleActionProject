using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void GoToScene()
    {
        SceneManager.LoadScene("AudioTest");
    }
}