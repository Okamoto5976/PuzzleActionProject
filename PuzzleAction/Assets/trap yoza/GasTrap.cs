using UnityEngine;
using System.Collections.Generic;

public class GasTrap : TrapBase
{
    private float m_DamageInterval = 1.0f;
    private float m_timer;

    private List<Entity> m_targets = new List<Entity>();

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

        //if (m_trapdata != null)
        //{
        //    m_destroyRange = m_trapdata.range;
        //}
        //else
        //{
        //    m_destroyRange = 9999f;
        //}

        
        //m_destroyRange = 9999f;
        //m_Timer = 0f;
    }

    private void Start()
    {
        m_damageData = SetDamageData();
    }

    private void Update()
    {
        m_timer += Time.deltaTime;

        if(m_timer > 1f)
        {
            m_timer = 0f;

            for (int i = 0; i < m_targets.Count; i++)
            {
                m_targets[i].TakeDamage(m_damageData);
            }
        }
    }

    protected override void OnHit()
    {

    }

    protected override void OnTriggerEnter(Collider other)
    {
        //if (!enabled) return;



        Entity target = other.GetComponentInParent<Entity>();

        if (target == null)
        {
            //Debug.Log("Entityではない");
            return;
        }

        if (target.Team == TeamType.Nature) return;

        //Debug.Log("Gas");

        //m_timer = 0;

        //float finalDamage = (m_trapdata != null) ? m_trapdata.damage : 1;

        if(!m_targets.Contains(target))
        {
            m_targets.Add(target);
        }


        //Debug.Log($"[GAS_BOX] {target.gameObject.name} に {finalDamage} ダメージ判定！");

    }

    private void OnTriggerExit(Collider other)
    {
        Entity target = other.GetComponentInParent<Entity>();

        if (target != null && m_targets.Contains(target))
        {
            m_targets.Remove(target);
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