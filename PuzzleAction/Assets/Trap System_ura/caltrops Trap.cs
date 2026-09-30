using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaltropTrap : MonoBehaviour
{
    [Header("Trap Data")]
    [SerializeField]
    private TrapData m_trapData;

    [Header("Team")]
    [SerializeField]
    private TeamType m_team;


    [Header("Damage Interval")]
    [SerializeField]
    private float m_damageInterval = 1.0f;


    // Areaì‡Ç…Ç¢ÇÈEntity
    private HashSet<Entity> m_targets =
        new HashSet<Entity>();


    private Coroutine m_damageCoroutine;

    private DamageData m_damageData;


    public void TrapInit(Entity owner, Vector3 dir)
    {
        m_damageData = new DamageData
        {
            Attack = m_trapData.m_trapAttack,

            CriticalRate =
                m_trapData.m_trapCriticalRate,

            CriticalDamage =
                m_trapData.m_trapCriticalDamage,

            BreakRate =
                m_trapData.m_trapBreakRate,

            Knockback =
                m_trapData.m_trapKnockBack,

            StunDuration =
                m_trapData.m_trapStunDuration,

            AttackDir = dir
        };
    }


    protected virtual void TrapArea(Entity target)
    {
        if (target == null)
            return;

        if (target.Team == m_team)
            return;

        target.TakeDamage(m_damageData);
    }


    private void OnTriggerEnter(Collider other)
    {
        Entity target =
            other.GetComponentInParent<Entity>();

        if (target == null)
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
            foreach (Entity target in m_targets)
            {
                TrapArea(target);
            }

            yield return new WaitForSeconds(
                m_damageInterval);
        }

        m_damageCoroutine = null;
    }


    private void StopDamage()
    {
        if (m_damageCoroutine == null)
            return;

        StopCoroutine(m_damageCoroutine);
        m_damageCoroutine = null;
    }


    private void OnDisable()
    {
        StopDamage();
        m_targets.Clear();
    }
}