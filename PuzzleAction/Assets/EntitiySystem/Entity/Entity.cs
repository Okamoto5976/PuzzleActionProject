using System.Collections.Generic;
using UnityEngine;

public enum TeamType
{
    Player,
    Enemy,
    Nature
}

[RequireComponent (typeof(Rigidbody))]
abstract public class Entity : MonoBehaviour
{
    #region Entity Status
    public float HP => m_status[StatusType.HP].Value;
    public float STR  => m_status[StatusType.Strength].Value; 
    public float KnockBack => m_status[StatusType.KnockBack].Value;
    public float DEF => m_status[StatusType.Defense].Value;
    public float Speed
    {
        get
        {
            float baseSpeed = m_status[StatusType.Speed].Value;

            float slowMultiplier = 1f - Swamp * (1f - SwampRes);
            slowMultiplier = Mathf.Clamp(slowMultiplier, 0.25f, 1f);

            float finalSpeed = (baseSpeed * slowMultiplier) - Slow;

            return Mathf.Max(finalSpeed, 0f);
        }
    }
    public float CriticalRate => m_status[StatusType.CriticalRate].Value;
    public float CriticalDamage => m_status[StatusType.CriticalDamage].Value;
    public float BreakRate => m_status[StatusType.BreakRate].Value;
    public float StunPower => m_status[StatusType.StunDuration].Value;
    public float PoisonRes => m_status[StatusType.PoisonRes].Value;
    public float StunRes => m_status[StatusType.StunRes].Value;
    public float SwampRes => m_status[StatusType.SwampRes].Value;
    public float GasRes => m_status[StatusType.GasRes].Value;
    public float BurnRes => m_status[StatusType.BurnRes].Value;
    public float Slow => m_status[StatusType.Slow].Value;
    public float Swamp => m_status[StatusType.Swamp].Value;
    public float Poison => m_status[StatusType.Poison].Value;
    public float Gas => m_status[StatusType.Gas].Value;
    public float Burn => m_status[StatusType.Burn].Value;
    public float Regenerate => m_status[StatusType.Regenerate].Value;
    public float Invincible => m_status[StatusType.Invincible].Value;
    #endregion

    public enum EntityState
    {
        Idle,
        Attack,
        Dead
    }
    protected EntityState m_currentState = EntityState.Idle;
    public EntityState CurrentState { get => m_currentState; }

    //component
    protected Rigidbody m_rb;
    protected Animator m_anim;

    protected EntityHP m_entityHP;

    protected EntityTemporaryBuffSystem m_buffSystem;

    ////SE
    //[SerializeField] 
    //protected AudioClip m_attackSE;

    //public AudioClip AttackSE => m_attackSE;

    //[SerializeField]
    //protected AudioClip m_damageSE;

    //public AudioClip DamageSE=> m_damageSE;

    //protected AudioSource m_audioSource;
    //public AudioSource AudioSource => m_audioSource;

    

    [SerializeField] protected TeamType m_team;
    public TeamType Team => m_team;
    
    [SerializeField] protected EntityData m_data;

    protected bool m_canMove;
    private bool m_isStun;
    private bool m_isInvincible;
    private bool m_isKnockBack;
    protected bool m_isEvading;
    protected Vector3 m_evadeDirection;

    private float m_stunTimer;
    private float m_invincibleTimer;
    private float m_knockbackTimer;

    protected float m_knockbackPower;
    protected float StunTimer
    {
        get => m_stunTimer;
        set
        {
            if (m_stunTimer > value) return;
            m_stunTimer = Mathf.Min(3f, value);
        }
    }

    //protected float InvincibleTimer
    //{
    //    get => m_invincibleTimer;
    //    set
    //    {
    //        if (m_invincibleTimer > value) return;
    //        m_invincibleTimer = Mathf.Min(3f, value);
    //    }
    //}

    protected float KnockBackTimer
    {
        get => m_knockbackTimer;
        set
        {
            if (m_knockbackTimer > value) return;
            m_knockbackTimer = Mathf.Min(1f, value);

            m_knockbackPower = Mathf.Clamp(value, 1f, 3f);
        }
    }

    public bool CanMove { get => m_canMove; }
    public bool IsStun => m_isStun || StunTimer > 0f;
    //public bool IsInvincible => m_isInvincible || InvincibleTimer > 0f;
    public bool IsInvincible => Invincible > 0f;
    public bool IsKnockBack => m_isKnockBack || KnockBackTimer > 0f;
    public bool IsEvading
    {
        get => m_isEvading;
        set
        {
            m_isEvading = value;
            m_evadeDirection = m_moveDir;
        }
    }


    //--------Status Buff-------------------
    protected Dictionary<StatusType, EntityStatus> m_status = new();

    private float m_flagTime;
    //-------velocity---------------------------

    protected Vector3 m_moveDir;
    protected Vector3 m_velocity;

    protected Vector3 m_knockBackVelocity;

    public Vector3 MoveDir { get => m_moveDir; }

    private bool m_isCheckState = false;

    [SerializeField] private AudioData m_stunSE;


    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_entityHP = GetComponent<EntityHP>();
        m_anim = GetComponentInChildren<Animator>();
        m_buffSystem=GetComponent<EntityTemporaryBuffSystem>();

        SetState();

        m_canMove = true;
    }

    #region Status
    public void SetState()
    {
        if (m_isCheckState) return;
        m_isCheckState = true;

        if (m_data == null)
        {
            Debug.LogError("Not EntityData");
            return;
        }
        m_status.Add(StatusType.HP, new EntityStatus(m_data.HP));
        m_status.Add(StatusType.Strength, new EntityStatus(m_data.STR));
        m_status.Add(StatusType.KnockBack, new EntityStatus(m_data.KnockBack));
        m_status.Add(StatusType.Defense, new EntityStatus(m_data.DEF));
        m_status.Add(StatusType.Speed, new EntityStatus(m_data.Speed));
        m_status.Add(StatusType.CriticalRate, new EntityStatus(m_data.CriticalRate));
        m_status.Add(StatusType.CriticalDamage, new EntityStatus(m_data.CriticalDamage));
        m_status.Add(StatusType.BreakRate, new EntityStatus(m_data.BreakRate));
        m_status.Add(StatusType.StunDuration, new EntityStatus(m_data.StunDuration));
        m_status.Add(StatusType.PoisonRes, new EntityStatus(m_data.PoisonRes));
        m_status.Add(StatusType.StunRes, new EntityStatus(m_data.StunRes));
        m_status.Add(StatusType.SwampRes, new EntityStatus(m_data.SwampRes));
        m_status.Add(StatusType.GasRes, new EntityStatus(m_data.GasRes));
        m_status.Add(StatusType.BurnRes, new EntityStatus(m_data.BurnRes));
        m_status.Add(StatusType.Slow, new EntityStatus(0f));
        m_status.Add(StatusType.Swamp, new EntityStatus(0f));
        m_status.Add(StatusType.Poison, new EntityStatus(0f));
        m_status.Add(StatusType.Gas, new EntityStatus(0f));
        m_status.Add(StatusType.Burn, new EntityStatus(0f));
        m_status.Add(StatusType.Regenerate, new EntityStatus(0f));
        m_status.Add(StatusType.Invincible, new EntityStatus(0f));
    }

    protected virtual void Start()
    {
    }

    public EntityStatus GetStatus(StatusType type)
    {
        return m_status[type];
    }
    #endregion


    
    private static readonly HashSet<StatusType> s_damageStatusTypes = new()
    {
        StatusType.Burn,
        StatusType.Poison,
        StatusType.Gas,
    };

    /// <summary>
    /// ステータス系のBuffを与える際の
    /// </summary>
    /// <param name="modifier"></param>
    /// <param name="buffID"></param>
    /// <param name="duration"></param>
    public void AddBuff(StatusModifier modifier, BuffID buffID, float duration)
    {
        if(m_currentState == EntityState.Dead) return;

        if(m_buffSystem == null)
        {
            return;
        }

        //ダメージバフの場合はこっち
        if(s_damageStatusTypes.Contains(modifier.m_statType))
        {
            AddDamageBuff(modifier, buffID, duration);
            return;
        }

        m_buffSystem.AddBuff(modifier, buffID, duration);
    }

    /// <summary>
    /// ダメージのあるBuffを与える際の
    /// </summary>
    /// <param name="modifier"></param>
    /// <param name="buffID"></param>
    /// <param name="duration"></param>
    private void AddDamageBuff(StatusModifier modifier, BuffID buffID, float duration)
    {
        if (m_currentState == EntityState.Dead) return;


        if (m_buffSystem == null)
        {
            return;
        }

        float value;
        float actualDuration;

        switch (modifier.m_statType)
        {
            case StatusType.Poison:
                value = modifier.m_value * (1f - PoisonRes);
                actualDuration = duration * (1f - PoisonRes);
                break;
            case StatusType.Gas:
                value = modifier.m_value * (1f - GasRes);
                actualDuration = duration * (1f - GasRes);
                break;
            case StatusType.Burn:
                value = modifier.m_value * (1f - BurnRes);
                actualDuration = duration * (1f - BurnRes);
                break;
            default:
                value = modifier.m_value;
                actualDuration = duration;
                break;
        }

        modifier.m_value = value;

        m_buffSystem.AddBuff(modifier, buffID, actualDuration);
    }

    /// <summary>
    /// EntityHPから呼ぶ
    /// スタンを付与する。StunResに応じて効果時間を軽減する。
    /// </summary>
    /// <param name="duration">基礎スタン時間</param>
    public void ApplyStun(float duration)
    {
        if (m_currentState == EntityState.Dead) return;


        float actualDuration = duration * (1f - Mathf.Clamp01(StunRes));

        if (actualDuration <= 0f) return;

        StunTimer = actualDuration;

        AudioManager.Instance.PlayAudio(m_stunSE);
    }

    //public void ApplyInvincible(float duration)
    //{
    //    InvincibleTimer = duration;
    //}

    public void ApplyKnockBack(Vector3 direction, float power)
    {
        if (m_currentState == EntityState.Dead) return;


        if (power <= 0f) return;

        KnockBackTimer = power;

        m_knockBackVelocity = direction;

    }

    //call Update-------------------------------------------------------
    protected virtual void UpdateFlag()
    {
        if (m_currentState == EntityState.Dead) return;


        m_stunTimer -= Time.deltaTime;
        m_invincibleTimer -= Time.deltaTime;
        m_knockbackTimer -= Time.deltaTime;

        m_flagTime += Time.deltaTime;

        #region Buff
        if (Gas > 0f)
        {
            if (m_flagTime > 1f)
            {
                BuffTakeDamage(StatusType.Gas, Gas);
            }
        }

        if (Poison > 0f)
        {
            if (m_flagTime > 1f)
            {
                BuffTakeDamage(StatusType.Poison, Poison);
            }
        }

        if (Burn > 0f)
        {
            if (m_flagTime > 1f)
            {
                BuffTakeDamage(StatusType.Burn, Burn);
            }
        }

        if(Regenerate > 0f)
        {
            if(m_flagTime > 1f)
            {
                HealHP(Regenerate);
            }
        }
        #endregion

        if (m_flagTime > 1f)
        {
            m_flagTime = 0f;
        }
    }
    //----------------------------------------------------------------------

    /// <summary>
    /// 
    /// </summary>
    /// <param name="speed">計算後の値を入力</param>
    [SerializeField] private LayerMask wallLayer;

    public void Move(Vector3 dir, float speed)
    {
        dir = dir.normalized;

        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 1.5f, wallLayer))
        {
            Debug.Log("かべにあたった");
            dir = Vector3.ProjectOnPlane(dir, hit.normal).normalized;
        }

        m_velocity = m_rb.linearVelocity;
        m_velocity.x = dir.x * speed;
        m_velocity.z = dir.z * speed;
        m_rb.linearVelocity = m_velocity;
    }

    //EntityをTakeDamageに
    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    public virtual void TakeDamage(DamageData data)
    {
        if (m_currentState == EntityState.Dead) return;

        if (IsInvincible) return;

        if (m_entityHP == null) return;

        m_entityHP.TakeDamage(data);
    }

    protected virtual void BuffTakeDamage(StatusType type, float damage)
    {
        if (m_currentState == EntityState.Dead) return;

        if (IsInvincible) return;

        if (m_entityHP == null) return;

        m_entityHP.TakeBuffDamage(type, damage);
    }

    public virtual void HealHP(float value)
    {
        if (m_currentState == EntityState.Dead) return;

        if (m_entityHP == null) return;

        m_entityHP.Heal(value);
    }

    public void ChangeState(EntityState newState)
    {
        if (m_currentState == EntityState.Dead) return;


        m_currentState = newState;
    }

    public void KillEntity()
    {
        if (m_currentState == EntityState.Dead) return;


        m_currentState = EntityState.Dead;
        SetCanMove(false);
    }

    public void SetCanMove(bool value) => m_canMove = value;
    public void SetIsStun(bool value) => m_isStun = value;
    public void SetIsInvincible(bool value) => m_isInvincible = value;
}