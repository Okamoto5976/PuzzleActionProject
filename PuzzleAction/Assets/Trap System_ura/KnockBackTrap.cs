using UnityEngine;

public class KnockBackTrap : TrapBase
{
    [Header("Life Time")]
    [SerializeField]
    private float m_lifeTime = 3f;

    private float m_timer;

    public override void TrapInit()
    {
        base.TrapInit();

        m_timer = 0f;

        m_damageData = new DamageData
        {
            Attack = 0,
            AttackType = m_attackType,

            Knockback = m_power,

            AttackDir = m_dir
        };
    }


    protected override void EntitySetUp()
    {
        m_timer = 0f;

        m_damageData = new DamageData
        {
            Attack = 0,
            AttackType = m_attackType,

            Knockback = m_owner.KnockBack,

            AttackDir = m_dir
        };
    }


    private void FixedUpdate()
    {
        OnMove(m_dir);

        
    }

    private void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= m_lifeTime)
        {
            m_timer = 0f;

            OnHit();
        }
    }


    protected override void OnHit()
    {
        OnReturnPool();
    }


    protected override void OnTriggerEnter(Collider other)
    {
        Entity target =
            other.GetComponent<Entity>();

        if (target == null)
            return;

        if (m_owner != null &&
            target == m_owner)
            return;

        target.TakeDamage(m_damageData);

        //EntityHP entityHP =
        //    target.GetComponent<EntityHP>();

        //if (entityHP == null)
        //    return;

        //entityHP.TakeDamage(m_damageData);

    }
}