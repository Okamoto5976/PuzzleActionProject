using UnityEngine;
using UnityEngine.SceneManagement;


public class ResetScene : MonoBehaviour
{
    [ContextMenu("Reset Scene")]
    public void ResetThisScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
