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
    Stun,
    //状態異常 Resistance　100％が基礎
    PoisonRes,      //毒耐性
    StunRes,　　　　//スタン耐性 スタンになる確率 スタン、ノックバックからの復帰速度
    SlowRes,　　　//鈍足耐性
    BlindRes　　　　//盲目耐性
}

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/Datas/EntityData")]
public class EntityData : ScriptableObject
{
    [SerializeField, Header("HP")]
    private float m_hp;
    [SerializeField, Header("STR")]
    private float m_str;
    [SerializeField, Header("KnockBack")]
    private float m_knockBack;
    [SerializeField, Header("DEF")]
    private float m_def;
    [SerializeField, Header("Speed")]
    private float m_speed;
    [SerializeField, Header("DashSpeed")]
    private float m_dashSpeed;
    [SerializeField, Header("CriticalRate")]
    private float m_criticalRate;
    [SerializeField, Header("CriticalDamage")]
    private float m_criticalDamage;
    [SerializeField, Header("DEX")]
    private float m_dex;
    [SerializeField, Header("AGI")]
    private float m_agi;
    [SerializeField, Header("Vision")]
    private float m_vision;
    [SerializeField, Header("BreakRate")]
    private float m_breakRate;
    [SerializeField, Header("Stun")]
    private float m_stun;
    [SerializeField, Header("Recover")]
    private float m_recover;
    [SerializeField, Header("Poison Resistance")]
    private float m_poisonRes;
    [SerializeField, Header("Stun Resistance")]
    private float m_stunRes;
    [SerializeField, Header("Slow Resistance")]
    private float m_slowRes;
    [SerializeField, Header("Blind Resistance")]
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
    public float Stun { get => m_stun; }
    public float PoisonRes { get => m_poisonRes; }
    public float StunRes { get => m_stunRes; }
    public float SlowRes { get => m_slowRes; }
    public float BlindRes { get => m_blindRes; }
}
