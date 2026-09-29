using UnityEngine;

public class RockTrap : TrapBase
{
    [SerializeField] private LayerMask m_hitLayers;

    private bool m_isInitialized;

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

            Attack = m_owner.STR,
            //HitRate
            CriticalRate = m_owner.CriticalRate,
            CriticalDamage = m_owner.CriticalDamage,
            BreakRate = m_owner.BreakRate,
            Knockback = m_owner.KnockBack,
            StunDuration = m_owner.StunPower,
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
        //break anim

        OnReturnPool();
    }

    protected override void OnTriggerEnter(Collider other)
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

        if (m_owner != null)
        {
            if (target.Team ==
                m_owner.Team)
                return;
        }

        target.TakeDamage(m_damageData);

        //Debug.Log(
        //    $"{other.name} Hit");

        //Destroy(gameObject);
        OnHit();
    }

    
}
