using UnityEngine;

public class KnockBackBomb : TrapBase
{
    [Header("Bomb Settings")]
    [SerializeField] private float m_Range = 4f;
    [SerializeField] private float  m_FuseTime= 3f;

    private bool m_isFuseActive=false;
    private float m_fuseTimer = 0f;

    [SerializeField] private EffectEventDataSO m_effectEventData;

    [SerializeField] private ParticleSystem m_fireParticle;

    protected override void OnHit()
    {
        Explode();
    }
    protected override void EntitySetUp()
    {
        m_isFuseActive = true;
        m_fuseTimer = 0f;

        if(m_fireParticle != null)m_fireParticle.Play();
    }

    private void FixedUpdate()
    {
        CheckDeadLine();
    }

    private void Update()
    {
        if (m_isFuseActive)
        {
            m_fuseTimer += Time.deltaTime;
            if(m_fuseTimer>=m_FuseTime)
            {
                m_isFuseActive = false;
                Explode();
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (m_isFuseActive) return;
        if (m_team == TeamType.Nature) return;

        Entity target = other.GetComponent<Entity>();
        if (target == null || target.Team == m_team) return;

        if (m_fireParticle != null) m_fireParticle.Play();
        m_isFuseActive = true;
    }

    private void Explode()
    {
        CreateDamageData();

        //”ÍˆÍ”»’è
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_Range);

        foreach (var hitCollider in hitColliders)
        {
            Entity target = hitCollider.GetComponent<Entity>();
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
        if (m_effectEventData != null)
        {
            Effect data = new Effect()
            {
                effectType = Enum_EffectType.Explosion,
                effectPos = transform.position + new Vector3(0f, 0.5f, 0f),
                effectRot = transform.rotation,
            };
                m_effectEventData.Raise(data);
        }
        OnReturnPool();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_Range);
    }
}
