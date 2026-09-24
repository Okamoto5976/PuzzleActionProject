using UnityEngine;

public class Bluffbomb_moto : TrapBase
{
    [Header("Bomb Settings")]
    [SerializeField] private float m_Range = 4f;
    [SerializeField] private float m_KnockbackPower = 15f;
    [SerializeField] private float m_StunDuration = 1f;
    [SerializeField] private float  m_FuseTime= 3f;

    private bool m_isFuseActive=false;
    private float m_fuseTimer = 0f;
    protected override void OnHit()
    {
        Explode();
    }
    protected override void EntitySetUp()
    {
        m_damageData = new DamageData()
        {
            Attack = 0f,
            AttackType = m_attackType,
            Knockback = m_KnockbackPower + (m_owner != null ? m_owner.KnockBack : 0f),
            StunDuration = m_StunDuration + (m_owner != null ? m_owner.StunPower : 0f),
            Attacker = m_owner
        };
        m_isFuseActive = true;
        m_fuseTimer = 0f;
    }

    public override void TrapInit(ItemRecieveData data)
    {
        base.TrapInit(data);

        m_damageData = new DamageData()
        {
           Attack=0f,
           AttackType=m_attackType,
           Knockback=m_KnockbackPower,
           StunDuration=m_StunDuration,
           Attacker=null
        };
        m_isFuseActive =false;
        m_fuseTimer=0f;
    }

    private void FixedUpdate()
    {
        CheckDeadLine();
    }

    private void Update()
    {
        if(m_isFuseActive)
        {
            m_fuseTimer += Time.deltaTime;
            if(m_fuseTimer>=m_FuseTime)
            {
                m_isFuseActive=false;
                Explode();
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        //base.OnTriggerEnter(other);
        if (m_team == TeamType.Nature) return;

        Entity target = other.GetComponent<Entity>();

        if(target != null )
        {
            if(target.Team==m_team)return;
        Explode();
        }
    }

    private void Explode()
    {
        //”ÍˆÍ”»’è
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_Range);

        foreach (var hitCollider in hitColliders)
        {
            Entity target = hitCollider.GetComponent<Entity>();
            if (target == null) continue;

            if (target.Team == m_team) continue;

            Vector3 knockbackDir = target.transform.position - transform.position;
            knockbackDir.y = 0f;

            if (knockbackDir.sqrMagnitude < 0.001f)
            {
                knockbackDir = m_dir;
            }
            m_damageData.AttackDir = knockbackDir.normalized;

            target.TakeDamage(m_damageData);
        }
            OnReturnPool();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_Range);
    }
}
