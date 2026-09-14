using UnityEngine;

public class DynamiteTrap : TrapBase
{
    [SerializeField] private float m_knockBackValue;
    [SerializeField] private float m_stunDuration;

    [SerializeField] private ParticleSystem m_fireParticle;
    [SerializeField] private ParticleSystem m_explosionParticle;

    private bool m_isTimer = false;

    protected override void EntitySetUp()
    {
        m_damageData = new DamageData
        {

            Attack = m_str,
            AttackType = m_attackType,
            CriticalRate = m_owner.CriticalRate,
            CriticalDamage = m_owner.CriticalDamage,
            BreakRate = m_owner.BreakRate,
            Knockback = m_knockBackValue,
            StunDuration = m_stunDuration,
            //Dirは　当たった時に設定
            //AttackDir = m_dir,

        };

        m_isTimer = true;
    }

    public override void TrapInit()
    {
        base.TrapInit();

        m_damageData = new DamageData
        {

            Attack = m_str,
            AttackType = m_attackType,
            Knockback = m_knockBackValue,
            StunDuration = m_stunDuration,
            //Dirは　当たった時に設定
            //AttackDir = m_dir,

        };
    }

    protected override void OnHit()
    {
        OnReturnPool();

    }

    protected override void OnTriggerEnter(Collider other)
    {
        Entity target = other.GetComponentInParent<Entity>();

        if (target == null)
            return;

        if (target.Team ==
            TeamType.Nature)
            return;

        if (target.Team == m_team) return;


            //if (m_owner != null && target.Team == m_owner.Team) return;

            //Debug.Log($"[DYNAMITE] {target.gameObject.name} が踏んだ！爆発！");

            // Cプールに戻す（あっちのTrapBaseに備わっているプール返却処理を呼ぶ）
            //target.TakeDamage(m_damageData);

            //OnHit();

            //gameObject.SetActive(false);
        
    }
}   