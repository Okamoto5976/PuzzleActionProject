using UnityEngine;

public class TestDisplaty : MonoBehaviour
{
    [SerializeField, Range(0.01f, 0.5f)] private float delay = 0.1f;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TextDisplay.Instance.ShowMessageGradually("konnitiha!", delay);
        }
    }
}