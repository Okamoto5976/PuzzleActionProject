using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaltropTrap : TrapBase
{
    [Header("Damage")]
    [SerializeField]
    private float m_damageInterval = 1.0f;


    // åªç›ÅAÇ‹Ç´Ç—ÇµÇÃîÕàÕì‡Ç…Ç¢ÇÈEntity
    private HashSet<Entity> m_targets =
        new HashSet<Entity>();


    private Coroutine m_damageCoroutine;


    protected override void EntitySetUp()
    {
        OnAddForce(m_dir, 5f);
         
        m_targets.Clear();

        if (m_damageCoroutine != null)
        {
            StopCoroutine(m_damageCoroutine);
            m_damageCoroutine = null;
        }

        //Damage
        m_damageData = new DamageData
        {

            Attack = m_str,
            AttackType = m_attackType,
            CriticalRate = m_owner.CriticalRate,
            CriticalDamage = m_owner.CriticalDamage,
            BreakRate = m_owner.BreakRate,
        };

    }


    protected override void OnHit()
    {
         
         
    }

    
    public override void TrapInit()
    {
        base.TrapInit();

        //DamageData
        m_damageData = new DamageData
        {

            Attack = m_str,
            AttackType = m_attackType,
            CriticalRate = m_owner.CriticalRate,
            CriticalDamage = m_owner.CriticalDamage,
            BreakRate = m_owner.BreakRate,
        };
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Entity target =
            other.GetComponent<Entity>();

        if (target == null)
            return;
         
        if (target == m_owner)
            return;
         
        if (target.Team == m_team)
            return;
         
        m_targets.Add(target);
         
        if (m_damageCoroutine == null)
        {
            m_damageCoroutine =
                StartCoroutine(DamageCoroutine());
        }
    }


    private void OnTriggerExit(Collider other)
    {
        Entity target =
            other.GetComponent<Entity>();

        if (target == null)
            return;
        
        m_targets.Remove(target);

         
        if (m_targets.Count == 0)
        {
            StopDamage();
        }
    }


    private IEnumerator DamageCoroutine()
    {
        while (m_targets.Count > 0)
        {
             
            foreach (Entity target in m_targets)
            {
                if (target == null)
                    continue;

                target.TakeDamage(m_damageData);
            }
          
            yield return new WaitForSeconds(
                m_damageInterval);
        }


        m_damageCoroutine = null;
    }


    private void StopDamage()
    {
        if (m_damageCoroutine != null)
        {
            StopCoroutine(m_damageCoroutine);
            m_damageCoroutine = null;
        }
    }


    private void OnDisable()
    {
        StopDamage();
        m_targets.Clear();
    }
}