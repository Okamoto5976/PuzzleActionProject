using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BoolEventSO", menuName = "Scriptable Objects/BoolEventSO")]
public class BoolEventSO : ScriptableObject
{
    private event Action<bool> m_event;

    public void Raise(bool d_event)
    {
    m_event?.Invoke(d_event);
    }

    public void Register(Action<bool> d_event)
    {
        m_event += d_event;
    }
    public void Unregister(Action<bool> d_event)
    {
        m_event -= d_event;
    }

    public void RaiseEvent(bool d_event)
    {
        m_event?.Invoke(d_event);
    }
}
