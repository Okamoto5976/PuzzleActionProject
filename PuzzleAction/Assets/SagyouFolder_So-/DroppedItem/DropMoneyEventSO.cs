using UnityEngine;
using System;

[CreateAssetMenu(fileName = "DropMoneyEventSO", menuName = "Scriptable Objects/Events/DropMoneyEventSO")]
public class DropMoneyEventSO : ScriptableObject
{
    public event Action<Vector3, int> m_event;

    /// <summary>
    /// ŠÖ”‚Ì‹N“®
    /// </summary>
    public void Raise(Vector3 pos, int value)
    {
        m_event?.Invoke(pos, value);
    }

    /// <summary>
    /// ŠÖ”‚Ì“o˜^
    /// </summary>
    /// <param name="_event"></param>
    public void Register(Action<Vector3, int> _event)
    {
        m_event += _event;
    }

    /// <summary>
    /// ŠÖ”‚Ì“o˜^‰ğœ
    /// </summary>
    /// <param name="_event"></param>
    public void Unregister(Action<Vector3, int> _event)
    {
        m_event -= _event;
    }
}
