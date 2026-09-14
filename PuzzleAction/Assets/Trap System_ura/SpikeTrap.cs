using System.Collections;
using UnityEngine;

public class SpikeTrap : TrapBase
{
    [Header("Trap")]
    [SerializeField]
    private float m_cooldown = 2.0f;

     
    private bool m_isActive = true;


    protected override void EntitySetUp()
    {
         
        m_damageData = new DamageData
        {
            Attack = m_str + m_owner.STR,
            AttackType = m_attackType,

            CriticalRate = m_owner.CriticalRate,
            CriticalDamage = m_owner.CriticalDamage,
            BreakRate = m_owner.BreakRate,

            Knockback = m_owner.KnockBack,
            StunDuration = m_owner.Stun,

            AttackDir = m_dir
        };

         
        m_isActive = true;
    }


    protected override void OnTriggerEnter(Collider other)
    {
         
        if (!m_isActive)
        {
            return;
        }

         
        Entity entity = other.GetComponent<Entity>();

        if (entity == null)
        {
            return;
        }

         
        if (entity.Team == Team)
        {
            return;
        }

        
        entity.TakeDamage(m_damageData);

         
        OnHit();

         
        m_isActive = false;

         
        StartCoroutine(Cooldown());
    }


    private IEnumerator Cooldown()
    {
         
        yield return new WaitForSeconds(m_cooldown);

        
        m_isActive = true;
    }


    protected override void OnHit()
    {
       
    }
}

