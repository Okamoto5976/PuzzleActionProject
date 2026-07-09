using UnityEngine;

public class TrapBase : Entity
{
    [Header("TrapData")]
    [SerializeField]
    protected TrapData m_trapdata;

    //direction
    protected Vector3 m_direction;

    //startPosition
    protected Vector3 m_startPosition;

    //owner
    protected Entity m_owner;

    //range
    protected float m_range;

    //basevalue
    protected float m_basevalue;

    private DamageData m_damageData;

    //Receive orientation

    private ReturnObjectToPool m_returnObjPool;


    protected override void Awake()
    {
        base.Awake();
        //Fix the rotation
        m_rb.constraints = RigidbodyConstraints.FreezeRotation;
        m_returnObjPool = GetComponent<ReturnObjectToPool>();

    }

    protected virtual void Setup()
    {
        m_startPosition =
            m_owner.transform.position;

        m_range =
            m_trapdata.range;
    }

    
    public virtual void Init(
        Entity owner,
        Vector3 dir,
        int baseValue)
    {
        m_owner = owner;

        m_direction =
            dir.normalized;

        transform.rotation =
            Quaternion.LookRotation(
                m_direction);

        m_basevalue =
            baseValue;

        m_damageData = new DamageData
        {

            Attack = STR + m_basevalue,
            AttackType = AttackType.None,
            //HitRate
            CriticalRate = owner.CriticalRate,
            CriticalDamage = owner.CriticalDamage,
            BreakRate = owner.BreakRate,
            Knockback = owner.KnockBack,
            Stun = owner.Stun,
            //Duration
            AttackDir = dir,
            //SE

        };

        Setup();
    }

    protected override void FixedUpdate()
    {
        m_moveDir =
            m_direction;

        base.FixedUpdate();

        CheckRange();
    }

    private void CheckRange()
    {
        float distance =
            Vector3.Distance(
                m_startPosition,
                transform.position);

        if (distance >= m_range)
        {
            if (m_returnObjPool == null)
            {
                m_returnObjPool = GetComponent<ReturnObjectToPool>();

            }
            m_returnObjPool.ReturnToPool();
        }
    }

    private void OnTriggerEnter(
        Collider other)
    {
        Entity target =
            other.GetComponentInParent<Entity>();

        if (target == null)
            return;

        if (target.Team ==
            TeamType.Nature)
            return;

        if (m_owner != null)
        {
            if (target.Team ==
                m_owner.Team)
                return;
        }

        target.TakeDamage(m_damageData);

        Debug.Log(
            $"{other.name} Hit");

        //Destroy(gameObject);
        if (m_returnObjPool == null)
        {
            m_returnObjPool = GetComponent<ReturnObjectToPool>();

        }
        m_returnObjPool?.ReturnToPool();
    }
}


