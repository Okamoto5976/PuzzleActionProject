using UnityEngine;

public class Gas_Trap : TrapBase
{
    private float m_DamageInterval = 1.0f;
    private float m_Timer;

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

        if (m_trapdata != null)
        {
            m_range = m_trapdata.range;
        }
        else
        {
            m_range = 9999f;
        }

        
        m_range = 9999f;
        m_Timer = 0f;
    }

    private void FixedUpdate()
    {
        m_Timer += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enabled) return;

        if (m_Timer >= m_DamageInterval)
        {
            Entity target = other.GetComponent<Entity>();
            if (target == null) return;

            if (target.Team == TeamType.Nature) return;
            if (m_owner != null && target.Team == m_owner.Team) return;

            m_Timer = 0;

            float finalDamage = (m_trapdata != null) ? m_trapdata.damage : 1;
            Debug.Log($"[GAS_BOX] {target.gameObject.name} に {finalDamage} ダメージ判定！");

            // HP減少
            /*
            DamageData data = new DamageData();
            data.Attack = (int)finalDamage;
            data.HitRate = 100f;
            data.AttackDir = (target.transform.position - transform.position).normalized;
            data.Attacker = m_owner;
            target.TakeDamage(data);
            */
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