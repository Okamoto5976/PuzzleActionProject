using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
abstract public class Entity : MonoBehaviour
{
    //Entity Status
    public float HP { get => m_status[StatusType.HP].Value; }
    public float STR { get => m_status[StatusType.Strength].Value; }
    public float KnockBack { get => m_status[StatusType.KnockBack].Value; }
    public float DEF { get => m_status[StatusType.Defense].Value; }
    public float Speed { get => m_status[StatusType.Speed].Value; }
    public float DashSpeed { get => m_status[StatusType.DashSpeed].Value; }
    public float CriticalRate { get => m_status[StatusType.CriticalRate].Value; }
    public float CriticalDamage { get => m_status[StatusType.CriticalDamage].Value; }
    public float DEX { get => m_status[StatusType.Dexterity].Value; }
    public float AGI { get => m_status[StatusType.Agility].Value; }
    public float Vision { get => m_status[StatusType.Vision].Value; }
    public float BreakRate { get => m_status[StatusType.BreakRate].Value; }
    public float Stun { get => m_status[StatusType.Stun].Value; }
    public float PoisonRes { get => m_status[StatusType.PoisonRes].Value; }
    public float StunRes { get => m_status[StatusType.StunRes].Value; }
    public float SlowRes { get => m_status[StatusType.SlowRes].Value; }
    public float BlindRes { get => m_status[StatusType.BlindRes].Value; }

    //状態
    //今のところ使ってない
    public enum EntityState
    {
        Idle,
        Attack,
        Damage,
        Dead
    }
    protected EntityState m_currentState = EntityState.Idle;
    public EntityState CurrentState { get => m_currentState; }


    //component
    protected Rigidbody m_rb;
    //protected Animator m_anim;
    protected EntityHP m_entityHP;

    //SE
    [SerializeField] 
    protected AudioClip m_attackSE;

    public AudioClip AttackSE => m_attackSE;

    [SerializeField]
    protected AudioClip m_damageSE;

    public AudioClip DamageSE=> m_damageSE;

    protected AudioSource m_audioSource;
    public AudioSource AudioSource => m_audioSource;

    public enum Teamtype
    {
        Player,
        Enemy,
        Nature
    }

    [SerializeField] protected Teamtype m_team;
    public Teamtype Team => m_team;

    [SerializeField] private EntityData m_data;

    //ゲームオーバーなどイベント中 移動キー制限
    protected bool m_canMove;
    //ノックバック中やスタン中　動けないフラグ（時間経過で回復）
    protected bool m_isStun;
    //無敵中　ダメージ後や　回避で
    protected bool m_isInvincible;

    public bool CanMove { get => m_canMove; }
    public bool IsStun { get => m_isStun; }
    public bool IsInvincible {  get => m_isInvincible; }

    protected float m_stunTime;


    protected Dictionary<StatusType, EntityStatus> m_status = new();

    protected Vector3 m_moveDir;
    protected Vector3 m_velocity;

    public Vector3 MoveDir { get => m_moveDir; }

    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_entityHP = GetComponent<EntityHP>();
        m_audioSource=GetComponent<AudioSource>();

        if (m_data == null) return;
        m_status.Add(StatusType.HP, new EntityStatus(m_data.HP));
        m_status.Add(StatusType.Strength, new EntityStatus(m_data.STR));
        m_status.Add(StatusType.KnockBack, new EntityStatus(m_data.KnockBack));
        m_status.Add(StatusType.Defense, new EntityStatus(m_data.DEF));
        m_status.Add(StatusType.Speed, new EntityStatus(m_data.Speed));
        m_status.Add(StatusType.DashSpeed, new EntityStatus(m_data.DashSpeed));
        m_status.Add(StatusType.CriticalRate, new EntityStatus(m_data.CriticalRate));
        m_status.Add(StatusType.CriticalDamage, new EntityStatus(m_data.CriticalDamage));
        m_status.Add(StatusType.Dexterity, new EntityStatus(m_data.DEX));
        m_status.Add(StatusType.Agility, new EntityStatus(m_data.AGI));
        m_status.Add(StatusType.Vision, new EntityStatus(m_data.Vision));
        m_status.Add(StatusType.BreakRate, new EntityStatus(m_data.BreakRate));
        m_status.Add(StatusType.Stun, new EntityStatus(m_data.Stun));
        m_status.Add(StatusType.PoisonRes, new EntityStatus(m_data.PoisonRes));
        m_status.Add(StatusType.StunRes, new EntityStatus(m_data.StunRes));
        m_status.Add(StatusType.SlowRes, new EntityStatus(m_data.SlowRes));
        m_status.Add(StatusType.BlindRes, new EntityStatus(m_data.BlindRes));
    }

    private void Start()
    {
        


    }

    public EntityStatus GetStatus(StatusType type)
    {
        return m_status[type];
    }

    protected virtual void FixedUpdate()
    {
        if (m_isStun) return;

        OnMove(m_moveDir);

    }

    private void Update()
    {
        if(m_isStun)
        {
            m_stunTime -= Time.deltaTime;

            if(m_stunTime <= 0)
            {
                m_isStun = false;

                ChangeState(EntityState.Idle);
            }
        }
    }

    protected void OnMove(Vector3 dir)
    {
        //Vector3 velocity = new Vector3(dir.x * speed, m_rb.linearVelocity.y, dir.z * speed);

        //m_rb.MovePosition
        //    (
        //        m_rb.position + 
        //        velocity * Time.fixedDeltaTime

        //    );
        dir = dir.normalized; //強制的にベクトルを1に

        m_velocity = m_rb.linearVelocity;

        m_velocity.x = dir.x * Speed;
        m_velocity.z = dir.z * Speed;

        m_rb.linearVelocity = m_velocity;

    }

    public virtual void TakeDamage(Entity attacker)//後々DamageDataとDamageResult
    {
        if (m_isInvincible) return;

        //追加した部分
        //HPを持たない(トラップ等)対応
        if (m_entityHP == null) return;

        m_entityHP.TakeDamage(attacker);
    }

    //状態変更用
    //特に使ってない
    public void ChangeState(EntityState newState)
    {
        m_currentState = newState;
    }

    public void SetCanMove(bool value) => m_canMove = value;
    public void SetIsStun(bool value) => m_isStun = value;
    public void SetIsInvincible(bool value) => m_isInvincible = value;

    //ノックバックの処理
    //EntityStateを変更
    public virtual void ApplyKnockBack(Vector3 direction,float power,float stunTime)
    {
        //無敵なら無効にしたい場合
        if (m_isInvincible) return;

        ChangeState(EntityState.Damage);

        //スタン状態
        m_isStun = true;
        m_stunTime = stunTime;

        direction.y = 0;

        //今の速度をリセット
        m_rb.linearVelocity = Vector3.zero;

        //力を加える
        m_rb.AddForce(direction.normalized*power,ForceMode.Impulse);
    }
}
//public bool IsEnemy(Entity other)
//{
//    if (other == null) return false;
//    return this.team != other.team;
//}

//public bool IsPlayer(Entity other)
//{
//    if (other == null) return false;
//    return true;


//追加した部分
//相手に攻撃が当たるかどうか
//public virtual bool CanHit(Entity other)
//{
//    if (other == null)
//    {
//        return false;
//    }

//    // 同チーム無効
//    if (Team == other.Team)
//    {
//        return false;
//    }

//    return true;
//}

//public bool IsSameTeam(Entity other)
//{
//    if (other == null) return false;
//    return this.m_team == other.m_team;
//}