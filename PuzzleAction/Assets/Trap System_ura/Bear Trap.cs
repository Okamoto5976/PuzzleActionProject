using UnityEngine;

public class BearTrap : TrapBase
{
    [Header("Bear Trap")]
    [SerializeField] private Collider m_damageCollider;

    [Header("Recovery")]
    [SerializeField] private float m_recoveryTime = 3.0f;

    private bool m_isActive;
    private bool m_isRecovering;

    protected override void EntitySetUp()
    {
        m_isActive = true;
        m_isRecovering = false;

        if(m_damageCollider != null)
        {
            m_damageCollider.enabled = true;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!m_isActive || m_isRecovering)
        {
            return;
        }

        Entity target = other.GetComponent<Entity>();

        if (target == null) 
        {
            return;
        }

        if (target.Team == m_team) return;

        m_damageData = new DamageData
        {
            Attack = m_str,
            AttackType = m_attackType,
            AttackDir = (target.transform.position - transform.position).normalized
        };

        target.TakeDamage(m_damageData);

        OnHit();

        m_isActive = false;
        m_isRecovering = true;

        if (m_damageCollider != null)
        {
            m_damageCollider.enabled = false;
        }

        Invoke(nameof(RecoverTrap), m_recoveryTime);
    }

    protected override void OnHit()
    {
       
    }

    private void RecoverTrap()
    {
        m_isActive = true;
        m_isRecovering = false;

        if (m_damageCollider != null)
        {
            m_damageCollider.enabled = true;
        }
    }

    public override void TrapInit(ItemRecieveData data)
    {
        base.TrapInit(data);

        CancelInvoke(nameof(RecoverTrap));

        m_isActive = true;
        m_isRecovering = false;

        if(m_damageCollider != null)
        {
            m_damageCollider.enabled = true;
        }
    }
}
