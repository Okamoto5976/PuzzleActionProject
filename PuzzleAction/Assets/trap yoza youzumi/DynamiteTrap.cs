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

    [SerializeField] private AudioData m_se;


    private bool m_isTimer = false;

    protected override void EntitySetUp()
    {
        m_team = TeamType.Nature;//上書き

        //m_fireParticle.gameObject.SetActive(true);

        m_fireParticle.Play();

        Invoke(nameof(Explode), m_explosionTimer);
    }

    public override void TrapInit()
    {
        base.TrapInit();

        m_isTimer = true;
    }

    protected override void OnHit()
    {
        //AttackHitBox box = new AttackHitBox()
        //{
        //    m_transform = transform,
        //    m_radius = m_radius,
        //};

        ////m_explosionParticle.Play();
        //Effect data = new Effect()
        //{
        //    effectType = Enum_EffectType.Explosion,
        //    effectPos = transform.position + new Vector3(0f,0.5f,0f),
        //    effectRot = transform.rotation,
        //};

        //m_effectEventData.Raise(data);
        //m_hitCollider.AttackCollider(m_damageData, m_team, box);


        //OnReturnPool();

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
        Invoke(nameof(Explode), m_explosionTimer);
    }

    private void Explode()
    {
        //範囲判定
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_radius);

        foreach (var hitCollider in hitColliders)
        {
            Entity target = hitCollider.GetComponentInParent<Entity>();
            if (target == null || target.Team == m_team) continue;

            Vector3 knockbackDir = target.transform.position - transform.position;
            knockbackDir.y = 0f;

            if (knockbackDir.sqrMagnitude < 0.001f)
            {
                knockbackDir = m_dir;
            }
            m_damageData.AttackDir = knockbackDir.normalized;

            target.TakeDamage(m_damageData);
        }

        Particle();

        AudioManager.Instance.PlayAudio(m_se);

        OnReturnPool();
    }

    private void Particle()
    {
        var pos = transform.position + new Vector3(0f, 0.5f, 0f);

        ParticleManager.Instance.PlayParticle(Enum_EffectType.Explosion, pos);
    }
}   