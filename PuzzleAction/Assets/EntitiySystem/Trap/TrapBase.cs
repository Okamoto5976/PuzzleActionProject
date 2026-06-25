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
            other.GetComponent<Entity>();

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

        // target.TakeDamage(
        //     STR + m_basevalue);

        Debug.Log(
            $"{other.name} Hit");

        //Destroy(gameObject);
    }
}


