using UnityEngine;

public class DynamiteTrap : Entity
{
    [SerializeField]
    private DynamiteTrapData m_data;

    private GameObject m_owner;

    private int m_damage;

    private int m_baseValue;

    private bool m_isTriggered;

    private float m_timer;

    public void Initialize(TrapUseData data)
    {
        transform.position = data.Position;

        m_owner = data.Owner;

        m_baseValue = data.BaseValue;

        m_damage =
            m_data.m_damage +
            m_baseValue;
    }

    private void Update()
    {
        if (!m_isTriggered)
        {
            return;
        }

        m_timer += Time.deltaTime;

        if (m_timer >= m_data.m_explosionDelay)
        {
            Explosion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_isTriggered)
        {
            return;
        }

        Entity target =
            other.GetComponent<Entity>();

        if (target == null)
        {
            return;
        }

        Entity ownerEntity =
            m_owner.GetComponent<Entity>();

        if (ownerEntity != null)
        {
            if (target.Team ==
                ownerEntity.Team)
            {
                return;
            }
        }

        TriggerExplosion();
    }

    public void TriggerExplosion()
    {
        if (m_isTriggered)
        {
            return;
        }

        m_isTriggered = true;

        Debug.Log("Dynamite Trigger");
    }

    private void Explosion()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                m_data.m_radius
            );

        foreach (Collider hit in hits)
        {
            Entity target =
                hit.GetComponent<Entity>();

            if (target == null)
            {
                continue;
            }

            Entity ownerEntity =
                m_owner.GetComponent<Entity>();

            if (ownerEntity != null)
            {
                if (target.Team ==
                    ownerEntity.Team)
                {
                    continue;
                }
            }

            target.TakeDamage(m_damage);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (m_data == null)
        {
            return;
        }

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            m_data.m_radius
        );
    }
}