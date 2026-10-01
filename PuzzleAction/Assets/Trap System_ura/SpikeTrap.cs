using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : TrapBase
{
    private bool m_isActive = true;

    [Header("Damage")]
    [SerializeField]
    private float m_damageInterval = 0.5f;

    private void Awake()
    {
        m_isActive = true;
    }


    private HashSet<Entity> m_targets =
        new HashSet<Entity>();

    private Coroutine m_damageCoroutine;

    public override void TrapInit()
    {
        base.TrapInit();

        m_targets.Clear();
    }

    protected override void EntitySetUp()
    {
        m_targets.Clear();

        if (m_damageCoroutine != null)
        {
            StopCoroutine(m_damageCoroutine);
            m_damageCoroutine = null;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Entity target =
            other.GetComponent<Entity>();

        if (target == null)
        {
            return;
        }

        if (target.Team == m_team)
        {
            return;
        }

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
        {
            return;
        }

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
                {
                    continue;
                }

                target.TakeDamage(m_damageData);
            }

            OnHit();

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

    protected override void OnHit()
    {

    }
}