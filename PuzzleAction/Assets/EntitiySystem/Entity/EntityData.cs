using UnityEngine;

public enum StatusType
{
    HP,
    Strength,
    KnockBack,
    Defense,
    Speed,
    DashSpeed,
    Slow,
    CriticalRate, 
    CriticalDamage,
    Agility,  //Enemy
    BreakRate,
    Stun, //Enemy stun power
    PoisonRes,
    StunRes,
    SlowRes,
}

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/Datas/EntityData")]
public class EntityData : ScriptableObject
{
    [SerializeField] private float m_hp;
    [SerializeField] private float m_str;
    [SerializeField] private float m_knockBack;
    [SerializeField] private float m_def;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_dashSpeed;
    [SerializeField,Range(0f,100f)] private float m_criticalRate;
    [SerializeField] private float m_criticalDamage;
    [SerializeField] private float m_agi;
    [SerializeField,Range(0f,100f)] private float m_breakRate;
    [SerializeField] private float m_stun;
    [SerializeField,Range(0f,1f)] private float m_recover;
    [SerializeField,Range(0f,1f)] private float m_poisonRes;
    [SerializeField,Range(0f,1f)] private float m_stunRes;
    [SerializeField,Range(0f,1f)] private float m_slowRes;


    public float HP { get => m_hp; }
    public float STR { get => m_str; }
    public float KnockBack { get => m_knockBack; }
    public float DEF { get => m_def; }
    public float Speed { get => m_speed; }
    public float DashSpeed { get => m_dashSpeed; }
    public float CriticalRate { get => m_criticalRate; }
    public float CriticalDamage { get => m_criticalDamage; }
    public float AGI { get => m_agi; }
    public float BreakRate { get => m_breakRate; }
    public float Stun { get => m_stun; }
    public float PoisonRes { get => m_poisonRes; }
    public float StunRes { get => m_stunRes; }
    public float SlowRes { get => m_slowRes; }
}
