using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectTrap : TrapBase
{
    [Header("Life Time")]
    [SerializeField] private float m_lifeTime = 5.0f;

    [Header("Stun Setting")]
    [SerializeField] private float m_stunDuration = 10.0f;
    [SerializeField] private float m_stunInterval = 1.0f;

    private readonly HashSet<Entity> m_targets =
        new HashSet<Entity>();

    private Coroutine m_stunCoroutine;
    private Coroutine m_lifeCoroutine;
    

    protected override void EntitySetUp()
    {
        m_targets.Clear();

        if (m_stunCoroutine != null)
        {
            StopCoroutine(m_stunCoroutine);
        }

        if (m_lifeCoroutine != null)
        {
            StopCoroutine(m_lifeCoroutine);
        }

        m_stunCoroutine = StartCoroutine(StunLoop());
        m_lifeCoroutine = StartCoroutine(LifeTimer());
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
    }


    private void OnTriggerExit(Collider other)
    {
        Entity target =
            other.GetComponentInParent<Entity>();

        if (target == null)
            return;

        m_targets.Remove(target);
    }


    private IEnumerator StunLoop()
    {
        while (true)
        {
            foreach (Entity target in m_targets)
            {
                if (target == null)
                    continue;

                m_damageData = new DamageData
                {
                    StunDuration = m_stunDuration
                };

                target.TakeDamage(m_damageData);
            }

            yield return new WaitForSeconds(m_stunInterval);
        }
    }


    private IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(m_lifeTime);

        OnReturnPool();
    }


    protected override void OnHit()
    {

    }


    private void OnDisable()
    {
        m_targets.Clear();

        if (m_stunCoroutine != null)
        {
            StopCoroutine(m_stunCoroutine);
            m_stunCoroutine = null;
        }

        if (m_lifeCoroutine != null)
        {
            StopCoroutine(m_lifeCoroutine);
            m_lifeCoroutine = null;
        }
    }
}