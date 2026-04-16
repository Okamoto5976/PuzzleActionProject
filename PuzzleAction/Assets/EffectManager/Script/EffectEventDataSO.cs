using System;
using System.Diagnostics.Tracing;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "EffectEventDataSO", menuName = "Scriptable Objects/EffectEventDataSO")]
public class EffectEventDataSO : ScriptableObject
{
    public event Action<EffectData> m_onEffect;   
    //public UnityEvent<EffectData> m_onEffect;   

    public void Raise(EffectData _data)
    {
        m_onEffect?.Invoke(_data);
    }
    public void Register(Action<EffectData> _data)
    {
        m_onEffect += _data;
    }
    public void Unregister(Action<EffectData> _data)
    {
        m_onEffect -= _data;
    }
}
