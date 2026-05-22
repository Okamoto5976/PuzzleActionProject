using UnityEngine;
using UnityEngine.Events;

public class BoolToUnityEvent : MonoBehaviour
{
    [SerializeField] private BoolEventSO m_event;
    [SerializeField] private UnityEvent<bool> m_unityEvent;

    private void OnEnable()
    {
        m_event.Register(DoEvent);
    }

    private void OnDisable()
    {
        m_event.Unregister(DoEvent);
    }

    private void DoEvent(bool state)
    {
        m_unityEvent.Invoke(state);
    }
}
