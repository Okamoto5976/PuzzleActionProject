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
    CriticalDamage, //クリティカルダメージ 倍率1.3とか
    Dexterity,　　　 //命中率　基本100
    Agility,        //攻撃速度(敵のステータス）
    Vision,         //明るさ(Playerのステータス）
    BreakRate,      //一撃率（9999ダメの確率）
    Stun,           //スタン攻撃大きさ　20が最大　耐性200が防ぐには必要
    //状態異常 Resistance　100％が基礎
    PoisonRes,      //毒耐性
    StunRes,　　　　//スタン耐性 スタンを引き算 スタン、ノックバックからの復帰速度
    SlowRes,　　　//鈍足耐性
    BlindRes　　　　//盲目耐性
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
    [SerializeField] private float m_criticalRate;
    [SerializeField] private float m_criticalDamage;
    [SerializeField] private float m_dex;
    [SerializeField] private float m_agi;
    [SerializeField] private float m_vision;
    [SerializeField] private float m_breakRate;
    [SerializeField] private float m_stun;
    [SerializeField] private float m_recover;
    [SerializeField] private float m_poisonRes;
    [SerializeField] private float m_stunRes;
    [SerializeField] private float m_slowRes;
    [SerializeField] private float m_blindRes;


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
