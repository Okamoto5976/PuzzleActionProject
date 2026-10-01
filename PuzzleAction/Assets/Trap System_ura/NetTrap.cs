using System.Collections.Generic;
using UnityEngine;

public class NetTrap : TrapBase
{
    [Header("Net Settings")]
    [SerializeField] private LayerMask m_hitLayers;

    private List<Entity> m_hitTargets = new List<Entity>();

    private bool m_isInitialized;


    private void FixedUpdate()
    {

        if (!m_isInitialized)
            return;

        OnAddForce(m_dir, m_power);

        m_isInitialized = false;
    }

    private void Update()
    {
        CheckDeadLine();
    }


    protected override void EntitySetUp()
    {
        m_hitTargets.Clear();

        m_rb.linearVelocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;

        m_isInitialized = true;
    }

    protected override void OnHit()
    {
        OnReturnPool();
    }


    protected override void OnTriggerEnter(Collider other)
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

        if (target.Team == TeamType.Nature) return;

        if (target.Team == m_team)
            return;

        if (m_hitTargets.Contains(target))
            return;

        m_hitTargets.Add(target);

        target.TakeDamage(m_damageData);
    }


    private void OnDisable()
    {
        m_hitTargets.Clear();

        m_isInitialized = false;
    }
}