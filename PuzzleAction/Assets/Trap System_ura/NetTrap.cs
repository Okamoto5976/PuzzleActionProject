using System.Collections.Generic;
using UnityEngine;

public class NetTrap : TrapBase
{
    [Header("Net Settings")]
    [SerializeField]
    private float m_stunDuration = 5.0f;
    
    private List<Entity> m_hitTargets = new List<Entity>();


    private void FixedUpdate()
    {
        //if (!m_isInitialized)
        //    return;

        
        //m_rb.linearVelocity =
        //    m_dir * m_speed;

    }

    private void Update()
    {
        CheckDeadLine();
    }


    protected override void EntitySetUp()
    {
        m_hitTargets.Clear();

        m_damageData = new DamageData
        {
            StunDuration = m_stunDuration,

        };

        m_rb.linearVelocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;


        OnAddForce(m_dir, 15f);
        //m_isInitialized = true;
    }

    protected override void OnHit()
    {
        OnReturnPool();
    }


    protected override void OnTriggerEnter(Collider other)
    {
        Entity target =
            other.GetComponentInParent<Entity>();

        if (target == null)
            return;

        if (target == m_owner)
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

        //m_isInitialized = false;
    }
}