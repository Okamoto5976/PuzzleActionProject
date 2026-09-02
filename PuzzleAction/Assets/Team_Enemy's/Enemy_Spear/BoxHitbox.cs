using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(BoxCollider))]
public class BoxHitbox : HitCollider
{
    private BoxCollider m_BoxCollider;

    [SerializeField] private bool m_isViewCollider;
    [SerializeField] private bool m_isVisible;

    private Coroutine m_viewCoroutine;

    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
    }

    public new void AttackCollider(DamageData data, TeamType myTeam, AttackHitBox attackHitBox)
    {
        Collider[] hits = Physics.OverlapBox(m_BoxCollider.center, m_BoxCollider.bounds.extents / 2, transform.rotation);

        foreach (var hit in hits)
        {
            Entity entity = hit.GetComponentInParent<Entity>();

            if (entity == null)
            {
                continue;
            }
            if (entity.Team == myTeam)
            {
                continue;
            }

            entity.TakeDamage(data);

            Debug.Log($"{entity.name}‚Éƒqƒbƒg");
        }

        if (m_isViewCollider)
        {
            if (m_viewCoroutine != null) return;

            m_viewCoroutine = StartCoroutine(ViewColliderTime());
        }
    }

    private IEnumerator ViewColliderTime()
    {
        m_isVisible = true;
        yield return new WaitForSeconds(0.5f);
        m_isVisible = false;

        m_viewCoroutine = null;

        yield break;
    }
}
