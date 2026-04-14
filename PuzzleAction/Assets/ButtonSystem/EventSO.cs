using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EventSO", menuName = "Scriptable Objects/EventSO")]
public class EventSO : ScriptableObject
{
    public event Action m_event;

    /// <summary>
    /// ŠÖ”‚Ì‹N“®
    /// </summary>
    public void Raise()
    {
        m_event?.Invoke();
    }

    /// <summary>
    /// ŠÖ”‚Ì“o˜^
    /// </summary>
    /// <param name="_event"></param>
    public void Register(Action _event)
    {
        m_event += _event;
    }

    /// <summary>
    /// ŠÖ”‚Ì“o˜^‰ğœ
    /// </summary>
    /// <param name="_event"></param>
    public void Unregister(Action _event)
    {
        m_event -= _event;
    }
}
