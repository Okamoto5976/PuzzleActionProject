using UnityEngine;

public enum StatusType
{
    HP,
    Strength,
    KnockBack,
    Defense,
    Speed,
    CriticalRate, 
    CriticalDamage,
    BreakRate,
    StunDuration, //Enemy stun power
    PoisonRes,
    StunRes,
    SwampRes,
    GasRes,
    BurnRes,
    Slow,
    Poison,
    Gas,
    Burn,
    Swamp,
    Regenerate,
    Invincible,
}

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/Datas/EntityData")]
public class EntityData : ScriptableObject
{
    [SerializeField] private float m_hp;
    [SerializeField] private float m_str;
    [SerializeField] private float m_knockBack;
    [SerializeField] private float m_def;
    [SerializeField] private float m_speed;
    [SerializeField,Range(0f,1f)] private float m_criticalRate;
    [SerializeField] private float m_criticalDamage;
    [SerializeField,Range(0f,1f)] private float m_breakRate;
    [SerializeField] private float m_stunDuration;
    [SerializeField,Range(0f,1f)] private float m_poisonRes;
    [SerializeField,Range(0f,1f)] private float m_stunRes;
    [SerializeField,Range(0f,1f)] private float m_swampRes;
    [SerializeField,Range(0f,1f)] private float m_gasRes;
    [SerializeField,Range(0f,1f)] private float m_burnRes;



    public float HP { get => m_hp; }
    public float STR { get => m_str; }
    public float KnockBack { get => m_knockBack; }
    public float DEF { get => m_def; }
    public float Speed { get => m_speed; }
    public float CriticalRate { get => m_criticalRate; }
    public float CriticalDamage { get => m_criticalDamage; }
    public float BreakRate { get => m_breakRate; }
    public float StunDuration { get => m_stunDuration; }
    public float PoisonRes { get => m_poisonRes; }
    public float StunRes { get => m_stunRes; }
    public float SwampRes { get => m_swampRes; }
    public float GasRes => m_gasRes;
    public float BurnRes => m_burnRes;
}
