using System.Collections.Generic;
using UnityEngine;

public class NetTrap : TrapBase
{
    [Header("Net Settings")]
    [SerializeField]
    private float m_stunDuration = 2.0f;

    [SerializeField]
    private float m_speed = 10.0f;


    
    private List<Entity> m_hitTargets =
        new List<Entity>();


    private bool m_isInitialized;


    private void FixedUpdate()
    {
        if (!m_isInitialized)
            return;

        
        m_rb.linearVelocity =
            m_dir * m_speed;

        m_isInitialized = false;
    }


    protected override void EntitySetUp()
    {
       
        m_hitTargets.Clear();


       
        m_damageData = new DamageData
        {
            Attack = m_str + m_owner.STR,
            AttackType = m_attackType,

            CriticalRate = m_owner.CriticalRate,
            CriticalDamage = m_owner.CriticalDamage,
            BreakRate = m_owner.BreakRate,
            Knockback = m_owner.KnockBack,

             
            StunDuration = m_stunDuration,

            AttackDir = m_dir
        };


        
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
        Entity target =
            other.GetComponentInParent<Entity>();

        if (target == null)
            return;


         
        if (target == m_owner)
            return;


         
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