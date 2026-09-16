using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectTrap : TrapBase
{
    [Header("Insect Setting")]
    [SerializeField] private Collider m_insectCollider;

    [Header("Stun Setting")]
    [SerializeField] private float m_stunDuration;
    [SerializeField] private float m_stunInterval;

    private readonly HashSet<Entity> m_target =
        new HashSet<Entity>();

    private Coroutine m_stunCoroutine;

    protected  void SetUp()
    {
        m_target.Clear();

        if(m_stunCoroutine != null)
        {
            StopCoroutine(m_stunCoroutine);
            m_stunCoroutine = null;
        }

        if(m_insectCollider != null)
        {
            m_insectCollider.enabled = false;
        }
    }

    protected override void EntitySetUp()
    {
        
    }

    public void Activate()
    {
        if (m_insectCollider == null)
            return;

        m_insectCollider.enabled = true;

        if (m_stunCoroutine == null) 
        {
            m_stunCoroutine = StartCoroutine(StunLoop());
        }
    }

    public void Deactivate()
    {
        if(m_insectCollider != null)
        {
            m_insectCollider.enabled = false;
        }

        m_target.Clear();

        if(m_stunCoroutine != null)
        {
            StopCoroutine (m_stunCoroutine);
            m_stunCoroutine = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity target = other.GetComponent<Entity>();

        if (target == null)
            return;

        if (target == m_owner)
            return;

        if (target.Team == m_team)
            return;

        m_target.Add(target);
    }

    private void OnTriggerExit(Collider other)
    {
        Entity target=other.GetComponent<Entity>();

        if (target == null) return;

        m_target.Remove(target);
    }

    private IEnumerator StunLoop()
    {
        while (true) 
        {
            foreach (Entity target in m_target)
            {
                if (target == null)
                    continue;

                m_damageData = new DamageData
                {
                    StunDuration = m_stunDuration,
                    Attacker = m_owner
                };

                target.TakeDamage(m_damageData);
            }
            yield return new WaitForSeconds(m_stunDuration);
        }
    }

    protected override void OnHit()
    {
        
    }
}
