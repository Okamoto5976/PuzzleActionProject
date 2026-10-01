using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectTrap : TrapBase
{
    [Header("Stun Setting")]
    [SerializeField] private float m_stunInterval = 1.0f;

    private readonly HashSet<Entity> m_targets =
        new HashSet<Entity>();

    private Coroutine m_stunCoroutine;

    protected override void EntitySetUp()
    {
        m_targets.Clear();

        if (m_stunCoroutine != null)
        {
            StopCoroutine(m_stunCoroutine);
        }

        m_stunCoroutine = StartCoroutine(StunLoop());
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Entity target =
            other.GetComponentInParent<Entity>();

        if (target == null)
            return;

        if (target.Team == TeamType.Nature) return;


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

                target.TakeDamage(m_damageData);
            }

            yield return new WaitForSeconds(m_stunInterval);
        }
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
    }
}