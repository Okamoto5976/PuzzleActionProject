using UnityEngine;

public class Bluffbomb : TrapBase
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
        m_damageData new DamageData();
        {
            Attack=0f,
                attacktype=m_attackType,
                Knockback=m_KnockbackPower+(m_owner!=null?m_owner.KnockBack:0f),
                StunDuration=m_StunDuration+m_owner!=null?m_owner.StunPower:0f),
                Attacker=m_owner
        };
        m_isFuseActive = true;
        m_fuseTimer = 0f;
    }

    public override void TrapInit()
    {
        base.TrapInit();

        m_damageData = new DamageData()
        {
           Attack=0f,
           AttackType=m_attackType,
           Knockback=m_KnockbackPower,
           StunDuration=m_StunDuration,
           Attacker=null
        };
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

        Entity target = other.GetComponent<Entity>();

        if(target != null )
        {
            if(target.Team==m_team)return;
        }

        Explode();
    }

    private void Explode()
    {
        //if (m_Effect != null)
        //{
        //    Instantiate(m_Effect, transform.position, Quaternion.identity);
        //}
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

            //target.ApplyKnockBack(knockbackDir.normalized, m_KnockbackPower,0f);
        }
            OnReturnPool();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_Range);
    }
}
