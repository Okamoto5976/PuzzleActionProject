using UnityEngine;

public class KnifeTrap : TrapBase
{
    [SerializeField] private float m_rate = 1f;
    [SerializeField] private LayerMask m_hitLayers;

    private void FixedUpdate()
    {
        OnMove(m_dir);
    }

    protected override void EntitySetUp()
    {
        m_damageData = new DamageData
        {

            Attack = m_owner.STR * m_rate,
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
    }

    protected override void OnHit()
    {
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

        if (target.Team ==
            TeamType.Nature)
            return;

        if (target.Team == m_team) return;

        target.TakeDamage(m_damageData);

        
        OnHit();
    }
}
