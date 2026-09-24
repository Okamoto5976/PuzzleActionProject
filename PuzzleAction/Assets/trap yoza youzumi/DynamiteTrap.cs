using UnityEngine;

public class DynamiteTrap : TrapBase
{
    [SerializeField] private float m_knockBackValue;
    [SerializeField] private float m_stunDuration;

    [SerializeField] private float m_explosionTimer = 1.5f;

    [SerializeField] private ParticleSystem m_fireParticle;
    //[SerializeField] private ParticleSystem m_explosionParticle;

    [Header("HitCollider")]
    [SerializeField] private HitCollider m_hitCollider;
    [SerializeField] private float m_radius;

    [SerializeField] private EffectEventDataSO m_effectEventData;


    private bool m_isTimer = false;

    protected override void EntitySetUp()
    {
        m_team = TeamType.Nature;//上書き

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

        Debug.Log("fire");
        m_fireParticle.Play();
        Invoke(nameof(OnHit), m_explosionTimer);
    }

    public override void TrapInit(ItemRecieveData data)
    {
        base.TrapInit(data);

        m_damageData = new DamageData
        {

            Attack = m_str,
            AttackType = m_attackType,
            Knockback = m_knockBackValue,
            StunDuration = m_stunDuration,
            //Dirは　当たった時に設定
            //AttackDir = m_dir,

        };


        m_isTimer = true;
    }

    protected override void OnHit()
    {
        AttackHitBox box = new AttackHitBox()
        {
            m_transform = transform,
            m_radius = m_radius,
        };

        //m_explosionParticle.Play();
        Effect data = new Effect()
        {
            effectType = Enum_EffectType.Explosion,
            effectPos = transform.position + new Vector3(0f,0.5f,0f),
            effectRot = transform.rotation,
        };

        m_effectEventData.Raise(data);
        m_hitCollider.AttackCollider(m_damageData, m_team, box);


        OnReturnPool();

    }

    //Use TrapArea
    protected override void OnTriggerEnter(Collider other)
    {
        if (!m_isTimer) return;

        Entity target = other.GetComponentInParent<Entity>();

        if (target == null)
            return;

        if (target.Team ==
            TeamType.Nature)
            return;

        m_isTimer = false;

        m_fireParticle.Play();
        Invoke(nameof(OnHit), m_explosionTimer);
    }
}   