using System;
using System.Diagnostics.Tracing;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Effect
{
    public GameObject m_effectPrefab;
    public Vector3 m_effectPos;
    public Quaternion m_effectRot;

    public float m_lifeTime;
}

[CreateAssetMenu(fileName = "EffectEventDataSO", menuName = "Scriptable Objects/EffectEventDataSO")]
public class EffectEventDataSO : ScriptableObject
{
    public event Action<Effect> m_onEffect;   
    //public UnityEvent<EffectData> m_onEffect;   

    public void Raise(Effect _data)
    {
        m_onEffect?.Invoke(_data);
    }
    public void Register(Action<Effect> _data)
    {
        m_onEffect += _data;
    }
    public void Unregister(Action<Effect> _data)
    {
        m_onEffect -= _data;
    }
}



