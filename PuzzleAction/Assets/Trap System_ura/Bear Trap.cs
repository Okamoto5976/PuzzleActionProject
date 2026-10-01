using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : TrapBase
{
    [Header("Recovery")]
    [SerializeField] private float m_recoveryTime = 3.0f;

    private bool m_isActive = true;

    private HashSet<Entity> m_targets = new HashSet<Entity>();

    private Coroutine m_recoveryCoroutine;

    protected override void EntitySetUp()
    {
        m_isActive = true;
        m_targets.Clear();

        if (m_recoveryCoroutine != null)
        {
            StopCoroutine(m_recoveryCoroutine);
            m_recoveryCoroutine = null;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!m_isActive)
        {
            return;
        }

        Entity target = other.GetComponentInParent<Entity>();

        if (target == null) return;
        if (target.Team == TeamType.Nature) return;
        if (target.Team == m_team) return;

  
        m_targets.Add(target);

        target.TakeDamage(m_damageData);

        OnHit();

        m_isActive = false;

        if (m_recoveryCoroutine != null)
        {
            StopCoroutine(m_recoveryCoroutine);
        }
        m_recoveryCoroutine = StartCoroutine(RecoveryCoroutine());
    }

    private void OnTriggerExit(Collider other)
    {
        Entity target = other.GetComponentInParent<Entity>();

        if (target == null)
        {
            return;
        }
        m_targets.Remove(target);
    }

    private IEnumerator RecoveryCoroutine()
    {
        yield return new WaitForSeconds(m_recoveryTime);

        m_isActive = true;
        m_recoveryCoroutine = null;
    }

    private void OnDisable()
    {
        if (m_recoveryCoroutine != null)
        {
            StopCoroutine(m_recoveryCoroutine);
            m_recoveryCoroutine = null;
        }
        m_targets.Clear();
    }

    protected override void OnHit()
    {

    }

    public override void TrapInit()
    {
        base.TrapInit();

        if (m_recoveryCoroutine != null)
        {
            StopCoroutine(m_recoveryCoroutine);
            m_recoveryCoroutine = null;
        }

        m_isActive = true;
        m_targets.Clear();
    }
}