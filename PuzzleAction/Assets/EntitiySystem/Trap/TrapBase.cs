using UnityEngine;

[RequireComponent(typeof(ReturnObjectToPool))]

public abstract class TrapBase : MonoBehaviour
{
    //component
    protected Rigidbody m_rb;
    protected Animator m_anim;

    protected TeamType m_team = TeamType.Nature;
    public TeamType Team => m_team;

    //velocityに応じて　その方を正面にするか
    [SerializeField] private bool m_isFowardDir;

    [SerializeField] protected TrapData m_trapData;

    //direction
    protected Vector3 m_dir;

    [SerializeField] protected float m_speed;
    protected float m_power;//use arrow

    //startPosition
    protected Vector3 m_startPosition;

    //owner
    protected Entity m_owner;

    //range
    protected float m_destroyRange;

    //basevalue
    protected DamageData m_damageData;

    //Receive orientation
    protected ReturnObjectToPool m_returnObjPool;

    protected Vector3 m_velocity;


    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_returnObjPool = GetComponent<ReturnObjectToPool>();

    }

    protected abstract void EntitySetUp();

    protected abstract void OnHit();

    //call when entity use item 
    public void Init(
        ItemRecieveData data)
    {
        m_owner = data.entity;
        m_team = m_owner.Team;

        m_dir = data.dir.normalized;

        gameObject.transform.position = data.pos + data.offset;

        if(m_isFowardDir)
        {
            transform.rotation =
            Quaternion.LookRotation(
                m_dir);
        }

        m_power = data.power;

        EntitySetUp();//例　秒数を設定し　時間経過で爆破など
    }

    //use TrapArea
    public virtual void TrapInit()
    {
        m_owner = null;

        m_dir = Vector3.zero;
        m_team = TeamType.Nature;
    }

    //TrapInitの際　overrideで上書きで対応
    protected virtual void CreateDamageData()
    {
        m_damageData = new DamageData
        {
            Attack = GetValue(m_trapData.m_attack, m_owner.STR + m_trapData.m_base),
            CriticalRate = GetValue(m_trapData.m_criticalRate, m_owner.CriticalRate),
            CriticalDamage = GetValue(m_trapData.m_criticalDamage, m_owner.CriticalDamage),
            BreakRate = GetValue(m_trapData.m_breakRate, m_owner.BreakRate),
            Knockback = GetValue(m_trapData.m_knockback, m_owner.KnockBack),
            StunDuration = GetValue(m_trapData.m_stunDuration, m_owner.StunPower),

            AttackDir = m_dir
        };
    }

    protected float GetValue(float trapValue, float ownerValue)
    {
        return trapValue >= 0f ? trapValue : ownerValue;
    }

    protected virtual void CreateTrapDamageData()
    {
        m_damageData = new DamageData
        {
            Attack = m_trapData.m_trapAttack,
            CriticalRate = m_trapData.m_trapCriticalRate,
            CriticalDamage = m_trapData.m_trapCriticalDamage,
            BreakRate = m_trapData.m_trapBreakRate,
            Knockback = m_trapData.m_trapKnockBack,
            StunDuration = m_trapData.m_trapStunDuration,

            AttackDir = m_dir
        };
    }

    protected StatusModifier SetModifier(BuffSetting buff)
    {
        StatusModifier modifier = new StatusModifier()
        {
            m_statType = buff.m_statusType,
            m_value = buff.m_value,
            m_modType = buff.m_modifierType,
        };

        return modifier;
    }

    protected void OnMove(Vector3 dir)
    {
        dir = dir.normalized;

        m_velocity = m_rb.linearVelocity;

        m_velocity.x = dir.x * m_speed;
        m_velocity.z = dir.z * m_speed;

        m_rb.linearVelocity = m_velocity;
    }

    protected void OnAddForce(Vector3 dir, float power)
    {
        dir = dir.normalized;

        m_rb.AddForce(dir * power, ForceMode.VelocityChange);
    }

    protected void CheckRange()
    {
        if (m_destroyRange == 0) return;

        float distance =
            Vector3.Distance(
                m_startPosition,
                transform.position);

        if (distance >= m_destroyRange)
        {
            OnReturnPool();
        }
    }

    protected void CheckDeadLine()
    {
        if (transform.position.y < -20f)
        {
            OnReturnPool();
        }
    }

    protected void OnReturnPool()
    {
        if (m_returnObjPool == null)
        {
            m_returnObjPool = GetComponent<ReturnObjectToPool>();

        }
        m_returnObjPool.ReturnToPool();
    }

    protected virtual void OnTriggerEnter(
        Collider other)
    {

    }

}


