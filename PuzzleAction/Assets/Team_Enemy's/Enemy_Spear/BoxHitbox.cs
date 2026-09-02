using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(BoxCollider))]
public class BoxHitbox : HitCollider
{
    private BoxCollider m_BoxCollider;

    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
    }

    public override void AttackCollider(DamageData data, TeamType myTeam, AttackHitBox attackHitBox)
    {
        Collider[] hits = Physics.OverlapBox((transform.rotation * m_BoxCollider.center) + transform.position, m_BoxCollider.bounds.extents, transform.rotation);


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
    }
}
