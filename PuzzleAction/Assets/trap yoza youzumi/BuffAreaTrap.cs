using System.Collections.Generic;
using UnityEngine;

public class BuffAreaTrap : TrapBase
{
    private List<Entity> m_HitTargets = new List<Entity>();

    private float m_timer;

    protected override void EntitySetUp()
    {
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

            for (int i = 0; i < m_HitTargets.Count; i++)
            {
                if (m_trapData.m_buffSetting.Count != 0)
                {
                    foreach (var buff in m_trapData.m_buffSetting)
                    {
                        if (buff.m_duration <= 0) continue;

                        var modifier = SetModifier(buff);

                        m_HitTargets[i].AddBuff(modifier, buff.m_buffID, buff.m_duration);

                    }
                }
            }
        }


    }

    protected override void OnTriggerEnter(Collider other)
    {
        Entity target = other.GetComponentInParent<Entity>();

        if (target == null) return;

        if (target.Team == TeamType.Nature) return;

        if (!m_HitTargets.Contains(target))
        {
            m_HitTargets.Add(target);

            if (m_trapData.m_buffSetting.Count != 0)
            {
                foreach (var buff in m_trapData.m_buffSetting)
                {
                    if (buff.m_duration <= 0) continue;

                    var modifier = SetModifier(buff);

                    target.AddBuff(modifier, buff.m_buffID, buff.m_duration);

                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity target = other.GetComponentInParent<Entity>();

        if (target != null && m_HitTargets.Contains(target))
        {
            m_HitTargets.Remove(target);
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
