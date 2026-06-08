using UnityEngine;

public class Gas_Trap : Entity
{
    [SerializeField]
    private GasTrapData m_data;

    private GameObject m_owner;

    private int m_damage;

    private int m_baseValue;

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
        m_timer += Time.deltaTime;

        if (m_timer >= m_data.m_damageInterval)
        {
            DamageTargets();

            m_timer = 0f;
        }
    }

    private void DamageTargets()
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
    }

    private void OnDrawGizmosSelected()
    {
        if (m_data == null)
        {
            return;
        }

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            transform.position,
            m_data.m_radius
        );
    }
}