using System.Collections;
using UnityEngine;

public class InsectTrap : TrapBase
{
    [Header("Insect Setting")]
    [SerializeField] private Collider m_insectCollider;

    [Header("Stun Setting")]
    [SerializeField] private float m_stunDuration = 5.0f;

    private Entity m_target;

    private Coroutine m_stunCoroutine;

    protected override void EntitySetUp()
    {
        m_target = null;

        if (m_stunCoroutine != null)
        {
            StopCoroutine(m_stunCoroutine);
            m_stunCoroutine = null;
        }

        if (m_insectCollider != null)
        {
            m_insectCollider.enabled = false;
        }
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
        if (m_insectCollider != null)
        {
            m_insectCollider.enabled = false;
        }

        m_target = null;

        if (m_stunCoroutine != null)
        {
            StopCoroutine(m_stunCoroutine);
            m_stunCoroutine = null;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Entity target = other.GetComponentInParent<Entity>();

        if (target == null)
            return;

        if (target == m_owner)
            return;

        if (target.Team == m_team)
            return;

        // Ç∑Ç≈Ç…1ëÃÇ¢ÇÈèÍçáÇÕñ≥éã
        if (m_target != null)
            return;

        m_target = target;
    }

    protected  void OnTriggerExit(Collider other)
    {
        Entity target = other.GetComponentInParent<Entity>();

        if (target == null)
            return;

        if (target == m_target)
        {
            m_target = null;
        }
    }

    private IEnumerator StunLoop()
    {
        while (true)
        {
            if (m_target != null)
            {
                m_damageData = new DamageData
                {
                    StunDuration = m_stunDuration,
                    Attacker = m_owner
                };

                m_target.TakeDamage(m_damageData);
            }

            yield return new WaitForSeconds(m_stunDuration);
        }
    }

    protected override void OnHit()
    {

    }
}