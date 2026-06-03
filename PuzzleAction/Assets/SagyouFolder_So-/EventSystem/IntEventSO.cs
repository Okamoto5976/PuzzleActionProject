using UnityEngine;
using System;

[CreateAssetMenu(fileName = "IntEventSO", menuName = "Scriptable Objects/IntEventSO")]
public class IntEventSO : ScriptableObject
{
    private event Action<int> m_event;

    public void Raise(int d_event)
    {
        m_event?.Invoke(d_event);
    }

    public void Register(Action<int> d_event)
    {
        m_event += d_event;
    }
    public void Unregister(Action<int> d_event)
    {
        m_event -= d_event;
    }

    public void RaiseEvent(int d_event)
    {
        m_event?.Invoke(d_event);
    }
}
