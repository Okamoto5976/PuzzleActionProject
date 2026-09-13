using UnityEngine;

public class ArrowTrap : TrapBase
{
    [SerializeField] private LayerMask m_hitLayers;

    private bool m_isInitialized;

    [Header("BuffSetting")]
    [SerializeField] private bool m_isBuff;

    [SerializeField] private float m_duration = 1f;
    [SerializeField] private StatusType m_statusType;

    private void FixedUpdate()
    {
        if (!m_isInitialized)
            return;

        OnAddForce(m_dir, m_power);

        m_isInitialized = false;
    }

    protected override void EntitySetUp()
    {
        m_damageData = new DamageData
        {

            Attack = m_str + m_owner.STR,
            AttackType = m_attackType,
            //HitRate
            CriticalRate = m_owner.CriticalRate,
            CriticalDamage = m_owner.CriticalDamage,
            BreakRate = m_owner.BreakRate,
            Knockback = m_owner.KnockBack,
            StunDuration = m_owner.Stun,
            //Duration
            AttackDir = m_dir,
            //SE

        };

        //OnAddForce(m_dir, m_power);
        m_rb.linearVelocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
        m_isInitialized = true;

    }

    protected override void OnHit()
    {
        OnReturnPool();
    }

    private StatusModifier SetModifier()
    {
        StatusModifier modifier = new StatusModifier()
        {
            m_statType = StatusType.CriticalRate,
            m_value = 20f,
            m_modType = ModifierType.Subtract,
        };

        return modifier;
    }


    protected override void OnTriggerEnter(
        Collider other)
    {
        if ((m_hitLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            OnHit();
            return;
        }

        Entity target =
            other.GetComponentInParent<Entity>();

        if (target == null)
            return;

        //if (target.Team ==
        //    TeamType.Nature)
        //    return;

        if (target.Team == m_team) return;


        //if (m_owner != null)
        //{
        //    if (target.Team ==
        //        m_owner.Team)
        //        return;
        //}

        target.TakeDamage(m_damageData);

        //Debug.Log(
        //    $"{other.name} Hit");

        //Destroy(gameObject);
        OnHit();
    }
}
