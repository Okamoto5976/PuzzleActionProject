using System.Collections.Generic;
using UnityEngine;

public class SwampTrap : TrapBase
{
    private List<Entity> m_SlowedTargets = new List<Entity>();

    private float m_timer;

    protected override void SetUp()
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

        if(m_timer > 1f)
        {
            m_timer = 0f;

            for (int i = 0; i < m_SlowedTargets.Count; i++)
            {
                var modifier = SetModifier();

                m_SlowedTargets[i].AddBuff(modifier, BuffID.Slow, 1.5f);

            }
        }

        
    }

    private StatusModifier SetModifier()
    {
        StatusModifier modifier = new StatusModifier()
        {
            m_statType = StatusType.Slow,
            m_value = 1.5f,
            m_modType = ModifierType.Multiply,
        };

        return modifier;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        //if (!enabled) return;

        Entity target = other.GetComponentInParent<Entity>();

        if (target == null) return;

        //if (target.Team == TeamType.Nature) return;
        //if (m_owner != null && target.Team == m_owner.Team) return;

        if (!m_SlowedTargets.Contains(target))
        {
            m_SlowedTargets.Add(target);
            //Debug.Log($"[SWAMP_BOX] {target.gameObject.name} が沼に入った（ここに減速処理を追加可能）");
            var modifier = SetModifier();

           target.AddBuff(modifier, BuffID.Slow, 1.5f);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity target = other.GetComponentInParent<Entity>();

        if (target != null && m_SlowedTargets.Contains(target))
        {
            m_SlowedTargets.Remove(target);
            //Debug.Log($"[SWAMP_BOX] {target.gameObject.name} が沼から脱出した");
        }
    }
    private void OnDrawGizmosSelected()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (box != null)
        {
            Gizmos.color = Color.green;
            Vector3 size = new Vector3(
                box.size.x * transform.lossyScale.x,
                box.size.y * transform.lossyScale.y,
                box.size.z * transform.lossyScale.z
            );
            Vector3 center = transform.position + transform.TransformDirection(box.center);
            Gizmos.DrawWireCube(center, size);
        }
    }
    
}

