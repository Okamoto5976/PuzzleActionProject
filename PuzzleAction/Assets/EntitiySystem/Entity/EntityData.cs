using UnityEngine;

public enum StatusType
{
    HP,　　　　　　 //体力
    Strength,　　　 //攻撃力
    KnockBack,　　　//ノックバック力
    Defense,　　　　//防御(ダメージやノックバックを弱める）
    Speed,　　　　　//スピード
    DashSpeed,　　　//ダッシュ
    CriticalRate,　 //クリティカル率
    CriticalDamage, //クリティカルダメージ
    Dexterity,　　　 //命中率
    Agility,        //攻撃速度(敵のステータス）
    Vision,         //明るさ(Playerのステータス）
    BreakRate,      //一撃率（9999ダメの確率）
    Recover,        //スタン、ノックバックからの復帰
    //状態異常 Resistance
    PoisonRes,      //毒耐性
    StunRes,　　　　//スタン耐性
    SlowRes,　　　//鈍足耐性
    BlindRes　　　　//盲目耐性
}

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/Datas/EntityData")]
public class EntityData : ScriptableObject
{
    [Header("HP")]
    private float m_hp;
    [Header("STR")]
    private float m_str;
    [Header("KnockBack")]
    private float m_knockBack;
    [Header("DEF")]
    private float m_def;
    [Header("Speed")]
    private float m_speed;
    [Header("DashSpeed")]
    private float m_dashSpeed;
    [Header("CriticalRate")]
    private float m_criticalRate;
    [Header("CriticalDamage")]
    private float m_criticalDamage;
    [Header("DEX")]
    private float m_dex;
    [Header("AGI")]
    private float m_agi;
    [Header("Vision")]
    private float m_vision;
    [Header("BreakRate")]
    private float m_breakRate;
    [Header("Recover")]
    private float m_recover;
    [Header("Poison Resistance")]
    private float m_poisonRes;
    [Header("Stun Resistance")]
    private float m_stunRes;
    [Header("Slow Resistance")]
    private float m_slowRes;
    [Header("Blind Resistance")]
    private float m_blindRes;


    public float HP { get => m_hp; }
    public float STR { get => m_str; }
    public float KnockBack { get => m_knockBack; }
    public float DEF { get => m_def; }
    public float Speed { get => m_speed; }
    public float DashSpeed { get => m_dashSpeed; }
    public float CriticalRate { get => m_criticalRate; }
    public float CriticalDamage { get => m_criticalDamage; }
    public float DEX { get => m_dex; }
    public float AGI { get => m_agi; }
    public float Vision { get => m_vision; }
    public float BreakRate { get => m_breakRate; }
    public float Recover { get => m_recover; }
    public float PoisonRes { get => m_poisonRes; }
    public float StunRes { get => m_stunRes; }
    public float SlowRes { get => m_slowRes; }
    public float BlindRes { get => m_blindRes; }
}
