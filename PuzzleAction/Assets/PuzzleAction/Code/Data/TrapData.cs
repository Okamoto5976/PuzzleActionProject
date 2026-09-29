using UnityEngine;

[CreateAssetMenu(fileName = "TrapData", menuName = "Scriptable Objects/Datas/TrapData")]
public class TrapData : ScriptableObject
{
    [Header("TrapData")]
    public float m_base = 0f;
    public float m_attack = -1f;
    public float m_criticalRate = -1f;
    public float m_criticalDamage = -1f;
    public float m_breakRate = -1f;
    public float m_knockback = -1f;
    public float m_stunDuration = -1f;

    [Header("TrapAreaData")]
    public float m_trapAttack = 0f;
    public float m_trapCriticalRate = 0f;
    public float m_trapCriticalDamage = 0f;
    public float m_trapBreakRate = 0f;
    public float m_trapKnockBack = 0f;
    public float m_trapStunDuration = 0f;

}
