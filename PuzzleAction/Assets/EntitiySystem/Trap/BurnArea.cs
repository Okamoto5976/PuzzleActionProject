using System.Collections.Generic;
using UnityEngine;

public class BurnArea : TrapBase
{
    private List<Entity> m_SlowedTargets = new List<Entity>();

    private float m_timer;

    protected override void EntitySetUp()
    {
        //if (m_owner != null)
        //{
        //    m_startPosition = m_owner.transform.position;
        //}
        //else
        //{
        //    m_startPosition = transform.position;
        //}

        //m_destroyRange = 9999f; 
    }

    protected override void OnHit()
    {

    }

    private void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer > 1f)
        {
            m_timer = 0f;

            for (int i = 0; i < m_SlowedTargets.Count; i++)
            {
                var modifier = SetModifier();

                m_SlowedTargets[i].AddBuff(modifier, BuffID.Burn, 1.5f);

            }
        }


    }

    private StatusModifier SetModifier()
    {
        StatusModifier modifier = new StatusModifier()
        {
            m_statType = StatusType.Burn,
            m_value = 2f,
            m_modType = ModifierType.Add,
        };

        return modifier;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Entity target = other.GetComponentInParent<Entity>();

        if (target == null) return;

        //if (target.Team == TeamType.Nature) return;
        //if (m_owner != null && target.Team == m_owner.Team) return;

        if (!m_SlowedTargets.Contains(target))
        {
            m_SlowedTargets.Add(target);
            var modifier = SetModifier();

            target.AddBuff(modifier, BuffID.Burn, 1.5f);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity target = other.GetComponentInParent<Entity>();

        if (target != null && m_SlowedTargets.Contains(target))
        {
            m_SlowedTargets.Remove(target);
            //Debug.Log($"[SWAMP_BOX] {target.gameObject.name} Ç™è¿Ç©ÇÁíEèoÇµÇΩ");
        }
    }
}
