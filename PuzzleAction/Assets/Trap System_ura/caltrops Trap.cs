using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaltropTrap : TrapBase
{
    [Header("Damage")]
    [SerializeField] private float m_damageInterval = 1.0f;

    private HashSet<Entity> m_targets =
        new HashSet<Entity>();

    private Coroutine m_damageCoroutine;

    protected override void EntitySetUp()
    {
        m_targets.Clear();

        if (m_damageCoroutine != null)
        {
            StopCoroutine(m_damageCoroutine);
            m_damageCoroutine = null;
        }
    }

    public override void TrapInit()
    {
        base.TrapInit();

        m_targets.Clear();
        if (m_damageCoroutine != null)
        {
            StopCoroutine(m_damageCoroutine);
            m_damageCoroutine = null;
        }
    }

    protected override void OnHit()
    {
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
            other.GetComponentInParent<Entity>();

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
            List<Entity> removeTargets = new List<Entity>();

            foreach (Entity target in m_targets)
            {
                if(target == null )
                {
                    continue;
                }

                if(!target.gameObject.activeInHierarchy)
                {
                    removeTargets.Add(target);
                    continue;
                }
                target.TakeDamage(m_damageData);
            }

            foreach (Entity target in removeTargets)
            {
                m_targets.Remove(target);
            }

            OnHit();
            yield return new WaitForSeconds(1.0f);
        }

        //m_damageData = null;
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