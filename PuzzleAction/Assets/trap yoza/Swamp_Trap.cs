using System.Collections.Generic;
using UnityEngine;

public class Swamp_Trap : TrapBase
{
    private List<Entity> m_SlowedTargets = new List<Entity>();

    protected override void Setup()
    {
        if (m_owner != null)
        {
            m_startPosition = m_owner.transform.position;
        }
        else
        {
            m_startPosition = transform.position;
        }

        m_range = 9999f; 
    }

    private void FixedUpdate()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        Entity target = other.GetComponent<Entity>();
        if (target == null) return;

        if (target.Team == TeamType.Nature) return;
        if (m_owner != null && target.Team == m_owner.Team) return;

        if (!m_SlowedTargets.Contains(target))
        {
            m_SlowedTargets.Add(target);
            Debug.Log($"[SWAMP_BOX] {target.gameObject.name} が沼に入った（ここに減速処理を追加可能）");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity target = other.GetComponent<Entity>();
        if (target != null && m_SlowedTargets.Contains(target))
        {
            m_SlowedTargets.Remove(target);
            Debug.Log($"[SWAMP_BOX] {target.gameObject.name} が沼から脱出した");
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

