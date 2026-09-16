using UnityEngine;

[RequireComponent(typeof(ReturnObjectToPool))]

public abstract class TrapBase : MonoBehaviour
{
    //component
    protected Rigidbody m_rb;
    protected Animator m_anim;

    protected TeamType m_team = TeamType.Nature;
    public TeamType Team => m_team;

    //[Header("TrapData")]
    //[SerializeField]
    //protected TrapData m_trapdata;

    //direction
    protected Vector3 m_dir;

    [SerializeField] protected float m_str;
    [SerializeField] protected float m_speed;
    [SerializeField] protected AttackType m_attackType;
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

    //m_startPosition =
    //    m_owner.transform.position;

    //m_range =
    //    m_trapdata.range;

    protected abstract void OnHit();

    //call when entity use item 
    public void Init(
        Entity owner,
        Vector3 dir)
    {
        m_owner = owner;
        m_team = m_owner.Team;

        m_dir =
            dir.normalized;

        transform.rotation =
            Quaternion.LookRotation(
                m_dir);

        //Œp³æ‚Å
        //m_damageData = new DamageData
        //{

        //    Attack = m_str + owner.STR,
        //    AttackType = m_attackType,
        //    //HitRate
        //    CriticalRate = owner.CriticalRate,
        //    CriticalDamage = owner.CriticalDamage,
        //    BreakRate = owner.BreakRate,
        //    Knockback = owner.KnockBack,
        //    StunDuration = owner.Stun,
        //    //Duration
        //    AttackDir = dir,
        //    //SE

        //};

        EntitySetUp();//—á@•b”‚ðÝ’è‚µ@ŽžŠÔŒo‰ß‚Å”š”j‚È‚Ç
    }

    public void PullInit(
        Entity owner,
        Vector3 dir,
        float power)
    {
        m_owner = owner;
        m_team = m_owner.Team;

        m_dir =
            dir.normalized;

        transform.rotation =
            Quaternion.LookRotation(
                m_dir);

        m_power = power;

        //Œp³æ‚Å
        //m_damageData = new DamageData
        //{

        //    Attack = m_str + owner.STR,
        //    AttackType = m_attackType,
        //    //HitRate
        //    CriticalRate = owner.CriticalRate,
        //    CriticalDamage = owner.CriticalDamage,
        //    BreakRate = owner.BreakRate,
        //    Knockback = owner.KnockBack,
        //    StunDuration = owner.Stun,
        //    //Duration
        //    AttackDir = dir,
        //    //SE

        //};

        EntitySetUp();//—á@•b”‚ðÝ’è‚µ@ŽžŠÔŒo‰ß‚Å”š”j‚È‚Ç
    }


    //use TrapArea
    public virtual void TrapInit()
    {
        m_owner = null;

        m_dir = Vector3.zero;
        m_team = TeamType.Nature;


    }

    //protected DamageData SetDamageData()
    //{
    //    DamageData data = new DamageData
    //    {

    //        Attack = m_str,
    //        AttackType = m_attackType,
    //        //HitRate

    //        //Duration
    //        //SE

    //    };

    //    return data;
    //}

    //private void FixedUpdate()
    //{
    //    m_moveDir =
    //        m_direction;

    //    CallMove();

    //    CheckRange();
    //}

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



        m_rb.AddForce(dir * power, ForceMode.Impulse);

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


