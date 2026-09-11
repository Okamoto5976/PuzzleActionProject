using UnityEngine;

public class Bluffbomb : TrapBase
{
    [Header("Bomb Settings")]
    [SerializeField] private float m_Range = 4f;
    [SerializeField] private float m_KnockbackPower = 15f;

    [Header("Visual Effects")]
    [SerializeField] private GameObject m_Effect;

    protected override void OnHit()
    {
        Explode();
    }
    protected override void SetUp()
    {
        
    }

    private void FixedUpdate()
    {
        OnMove(m_dir);
        CheckRange();
        CheckDeadLine();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        Entity target = other.GetComponent<Entity>();

        if(target != null )
        {
            if(target.Team==m_team)return;
        }

        Explode();
    }

    private void Explode()
    {
        if (m_Effect != null)
        {
            Instantiate(m_Effect, transform.position, Quaternion.identity);
        }
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

            target.ApplyKnockBack(knockbackDir.normalized, m_KnockbackPower,0f);
        }
            OnReturnPool();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_Range);
    }
}
