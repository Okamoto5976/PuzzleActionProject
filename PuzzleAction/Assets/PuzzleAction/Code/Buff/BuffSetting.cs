using UnityEngine;

[System.Serializable]
public class BuffSetting
{
    public float m_value;
    public StatusType m_statusType;//what status? HP, Strength
    public ModifierType m_modifierType;//what mod? Add, Multiply

    [Header("----Active Buff Setting ----")]
    public float m_duration;
    public BuffID m_buffID;
}
