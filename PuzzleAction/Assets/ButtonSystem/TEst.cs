using UnityEngine;

public class TEst : MonoBehaviour
{
    [SerializeField] private EventSO m_eventSO;

    private void OnEnable()
    {
        m_eventSO.Register(Test);
    }

    private void OnDisable()
    {
        m_eventSO.Unregister(Test);
    }

    private void Test()
    {
        Debug.Log("aaaaaaaaa");
    }
}
