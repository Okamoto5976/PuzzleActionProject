using UnityEngine;

public class Dynamite_Trap : TrapBase
{
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
    }

    protected override void FixedUpdate()
    {
        // 移動処理をキャンセル
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        Entity target = other.GetComponent<Entity>();
        if (target != null && target.Team != TeamType.Nature)
        {
            if (m_owner != null && target.Team == m_owner.Team) return;

            Debug.Log($"[DYNAMITE] {target.gameObject.name} が踏んだ！爆発！");

            // プールに戻す（あっちのTrapBaseに備わっているプール返却処理を呼ぶ）
            gameObject.SetActive(false);
        }
    }
    private void OnDrawGizmosSelected()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (box != null)
        {
            Gizmos.color = Color.green;
            Vector3 center = transform.position + transform.TransformDirection(box.center);
            Gizmos.DrawWireCube(center, box.size);
        }
    }
}   