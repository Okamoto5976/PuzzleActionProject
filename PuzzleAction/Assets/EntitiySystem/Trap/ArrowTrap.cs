using UnityEngine;

public class ArrowTrap : TrapBase
{
    [SerializeField] private float m_power;

    [SerializeField] private LayerMask m_hitLayers;

    private bool m_isInitialized;

    private void FixedUpdate()
    {
        if (!m_isInitialized)
            return;

        OnAddForce(m_dir, m_power);

        m_isInitialized = false;
    }

    protected override void SetUp()
    {
        //OnAddForce(m_dir, m_power);
        m_rb.linearVelocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
        m_isInitialized = true;

    }

    protected override void OnHit()
    {
        OnReturnPool();
    }


    protected override void OnTriggerEnter(
        Collider other)
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

        //if (target.Team ==
        //    TeamType.Nature)
        //    return;

        if (m_owner != null)
        {
            if (target.Team ==
                m_owner.Team)
                return;
        }

        target.TakeDamage(m_damageData);

        //Debug.Log(
        //    $"{other.name} Hit");

        //Destroy(gameObject);
        OnHit();
    }
}
