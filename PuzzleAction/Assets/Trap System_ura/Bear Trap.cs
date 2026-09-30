using UnityEngine;

public class BearTrap : MonoBehaviour
{
    [Header("Trap Data")]
    [SerializeField] private TrapData m_trapData;

    [Header("Recovery")]
    [SerializeField] private float m_recoveryTime = 3.0f;

    private Collider m_collider;
    private bool m_isActive = true;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_isActive)
        {
            return;
        }

        Entity entity = other.GetComponentInParent<Entity>();

        if (entity == null)
        {
            return;
        }

        DamageData damageData = new DamageData
        {
            Attack = m_trapData.m_trapAttack,
            CriticalRate = m_trapData.m_trapCriticalRate,
            CriticalDamage = m_trapData.m_trapCriticalDamage,
            BreakRate = m_trapData.m_trapBreakRate,
            Knockback = m_trapData.m_trapKnockBack,
            StunDuration = m_trapData.m_trapStunDuration,
            AttackDir = (entity.transform.position - transform.position).normalized
        };

        entity.TakeDamage(damageData);

        // ˆê“x”­“®‚µ‚½‚ç–³Œø‰»
        m_isActive = false;
        m_collider.enabled = false;

        // ˆê’èŽžŠÔŒã‚É•œŠˆ
        Invoke(nameof(RecoverTrap), m_recoveryTime);
    }

    private void RecoverTrap()
    {
        m_isActive = true;
        m_collider.enabled = true;
    }
}